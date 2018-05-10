using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Utilities.SeedWork
{
    public interface IAsyncRepository<T> where T : IAggregateRoot
    {
        ClaimsPrincipal User { set; }
        IUnitOfWork UnitOfWork { get; }

        Task<T> GetByIdAsync(int id);
        Task<T> GetSingleBySpecAsync(ISpecification<T> spec, bool trackChanges = false);
        Task<List<T>> ListAllAsync();
        Task<List<T>> ListAsync(ISpecification<T> spec, bool trackChanges = false);
        Task<RecordCounts> CountAsync(ISpecification<T> spec);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<bool> DeleteAsync(T entity);

        #region Generic CRUD Methods

        Task<TEntity> GetByIdAsync<TEntity>(int id) where TEntity : Entity;
        Task<TEntity> AddAsync<TEntity>(TEntity entity) where TEntity : Entity;
        Task<TEntity> UpdateAsync<TEntity>(TEntity entity) where TEntity : Entity;
        Task<bool> DeleteAsync<TEntity>(int id) where TEntity : Entity;
        Task<bool> DeleteAsync<TEntity>(TEntity entity) where TEntity : Entity;

        #endregion
    }
}
