using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Utilities.Extensions;

namespace Utilities.SeedWork
{
    public abstract class RepositoryBase<T> : IAsyncRepository<T> where T : class, IAggregateRoot
    {
        protected readonly DbContext Context;

        public ClaimsPrincipal User { set => SetUser(value); }

        protected abstract void SetUser(ClaimsPrincipal user);

        protected RepositoryBase(DbContext context)
        {
            Context = context;
        }

        #region IAsyncRepository Implementation

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await Context.Set<T>().FindAsync(id);
        }

        public async Task<T> GetSingleBySpecAsync(ISpecification<T> spec, bool trackChanges = false, bool ignoreQueryFilters = false)
        {
            return (await ListAsync(spec, trackChanges, ignoreQueryFilters)).FirstOrDefault();
        }

        public async Task<List<T>> ListAllAsync()
        {
            return await Context.Set<T>().AsNoTracking().ToListAsync();
        }

        public async Task<List<T>> ListAsync(ISpecification<T> spec, bool trackChanges = false, bool ignoreQueryFilters = false)
        {
            // fetch a Queryable that includes all expression-based includes
            var queryableResultWithIncludes = spec.Includes
                .Aggregate(Context.Set<T>().AsQueryable(),
                    (current, include) => current.Include(include));

            // modify the IQueryable to include any string-based include statements
            var result = spec.IncludeStrings
                .Aggregate(queryableResultWithIncludes,
                    (current, include) => current.Include(include));

            // Apply criteria to the query using the specification's criteria expression
            if (spec.Criteria != null)
                result = result.Where(spec.Criteria);

            if (spec.Model != null)
            {
                // Apply ordering 
                result = result.OrderBy(spec.Model);

                // Apply record selection where applicable
                if (spec.Model.start != -1)
                    result = result.Skip(spec.Model.start);
                if (spec.Model.length != -1)
                    result = result.Take(spec.Model.length);
            }

            if (!trackChanges)
                result = result.AsNoTracking();

            if (ignoreQueryFilters) //TODO: does this actually work as intended i.e. are deleted records included? How about deleted child records?
                result = result.IgnoreQueryFilters();

            // Return results
            return await result.ToListAsync();
        }

        public async Task<RecordCounts> CountAsync(ISpecification<T> spec)
        {
            var result = new RecordCounts
            {
                FilteredRecords = await Context.Set<T>().AsQueryable()
                    .Where(spec.Criteria)
                    .CountAsync(),
                TotalRecords = await Context.Set<T>().AsQueryable()
                    .CountAsync()
            };
            return result;
        }

        public async Task<T> AddAsync(T entity)
        {
            Context.Set<T>().Add(entity);
            await Context.SaveChangesAsync();
            return entity;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            Context.Set<T>().Update(entity);
            await Context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(T entity)
        {
            Context.Set<T>().Remove(entity);
            var rowsAffected = await Context.SaveChangesAsync();
            return rowsAffected > 0;
        }

        #region Generic Type CRUD Methods

        public async Task<TEntity> GetByIdAsync<TEntity>(int id) where TEntity : Entity
        {
            return await Context.Set<TEntity>().FindAsync(id);
        }

        public async Task<TEntity> AddAsync<TEntity>(TEntity entity) where TEntity : Entity
        {
            Context.Add(entity);
            await Context.SaveChangesAsync();
            return entity;
        }

        public async Task<TEntity> UpdateAsync<TEntity>(TEntity entity) where TEntity : Entity
        {
            Context.Set<TEntity>().Update(entity);
            await Context.SaveChangesAsync();
            return entity;
        }

        public async Task<TEntity> WriteAsync<TEntity>(TEntity values) where TEntity : Entity
        {
            if (values.IsTransient())
            {
                // Create
                var newEntity = await AddAsync(values);
                return newEntity;
            }

            // Update
            var entity = await GetByIdAsync<TEntity>(values.Id);
            if (entity == null) return null;

            entity.CopyValues(values);
            await UpdateAsync(entity);
            return entity;
        }

        public async Task<bool> DeleteAsync<TEntity>(int id) where TEntity : Entity
        {
            var entity = await GetByIdAsync<TEntity>(id);
            return await DeleteAsync(entity);
        }

        public async Task<bool> DeleteAsync<TEntity>(TEntity entity) where TEntity : Entity
        {
            Context.Set<TEntity>().Remove(entity);
            var rowsDeleted = await Context.SaveChangesAsync();
            return rowsDeleted > 0;
        }

        #endregion

        #endregion
    }
}
