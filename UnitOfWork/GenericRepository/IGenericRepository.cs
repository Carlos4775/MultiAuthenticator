using System.Linq.Expressions;

namespace UnitOfWork.GenericRepository
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAsync(IList<string>? includes = null);
        Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate, IList<string>? includes = null);
        Task<TEntity?> GetByAsync(Expression<Func<TEntity, bool>> predicate, IList<string>? includes = null);
        void Post(TEntity entity);
        void Post(IEnumerable<TEntity> entity);
        void Put(TEntity entity);
        IQueryable<TEntity> GetQueryable(IList<string>? includes = null);
    }
}
