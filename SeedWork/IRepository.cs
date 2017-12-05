using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities.SeedWork;

namespace Lexor.Utilities.SeedWork
{
    public interface IRepository<T> where T : IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }

        #region Asynchronous Interface

        Task<T> GetByIdAsync(int id);
        Task<T> GetSingleBySpecAsync(ISpecification<T> spec);
        Task<List<T>> ListAllAsync();
        Task<List<T>> ListAsync(ISpecification<T> spec);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);

        #endregion

        #region Synchronous Interface

        //T GetById(int id);
        //T GetSingleBySpec(ISpecification<T> spec);
        //IEnumerable<T> ListAll();
        //IEnumerable<T> List(ISpecification<T> spec);
        //T Add(T entity);
        //void Update(T entity);
        //void Delete(T entity);

        #endregion
    }
}
