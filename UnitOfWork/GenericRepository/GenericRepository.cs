using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace UnitOfWork.GenericRepository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly DbContext _dbContext;

        public GenericRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<TEntity>> GetAsync(IList<string>? includes = null)
        {
            IQueryable<TEntity> query = _dbContext.Set<TEntity>();

            if (includes != null)
            {
                foreach (string incude in includes)
                {
                    query = query.Include(incude);
                }
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate, IList<string>? includes = null)
        {
            IQueryable<TEntity> query = _dbContext.Set<TEntity>().Where(predicate);

            if (includes != null)
            {
                foreach (string incude in includes)
                {
                    query = query.Include(incude);
                }
            }

            return await query.ToListAsync();
        }

        public async Task<TEntity?> GetByAsync(Expression<Func<TEntity, bool>> predicate, IList<string>? includes = null)
        {
            IQueryable<TEntity> query = _dbContext.Set<TEntity>().Where(predicate);

            if (includes != null)
            {
                foreach (string incude in includes)
                {
                    query = query.Include(incude);
                }
            }

            return await query.FirstOrDefaultAsync();
        }

        public void Post(TEntity entity)
        {
            _dbContext.Add(entity);
        }

        public void Post(IEnumerable<TEntity> entity)
        {
            _dbContext.AddRange(entity);
        }

        public void Put(TEntity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public IQueryable<TEntity> GetQueryable(IList<string>? includes = null)
        {
            IQueryable<TEntity> queryable = _dbContext.Set<TEntity>();

            if (includes != null)
            {
                foreach (string incude in includes)
                {
                    queryable = queryable.Include(incude);
                }
            }

            return queryable;
        }
    }
}
