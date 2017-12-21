using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities.SeedWork;

namespace Lexor.Utilities.SeedWork
{
    public interface IRepository<T> where T : IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }

        Task<T> GetByIdAsync(int id);
        Task<T> GetSingleBySpecAsync(ISpecification<T> spec);
        Task<List<T>> ListAllAsync();
        Task<List<T>> ListAsync(ISpecification<T> spec);
        Task<RecordCounts> CountAsync(ISpecification<T> spec);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<bool> DeleteAsync(T entity);

        #region Generic CRUD Methods

        Task<TEntity> GetByIdAsync<TEntity>(int id) where TEntity : Entity;
        //Task<TEntity> AddAsync<TEntity>(TEntity entity) where TEntity : Entity;
        Task<TEntity> UpdateAsync<TEntity>(TEntity entity) where TEntity : Entity;
        Task<bool> DeleteAsync<TEntity>(int id) where TEntity : Entity;
        Task<bool> DeleteAsync<TEntity>(TEntity entity) where TEntity : Entity;

        #endregion
    }
}
