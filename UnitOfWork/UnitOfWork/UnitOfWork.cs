using Data;
using Microsoft.EntityFrameworkCore;
using UnitOfWork.GenericRepository;

namespace UnitOfWork.UnitOfWork
{
    public class UnitOfWorksService : IUnitOfWorksService
    {
        private readonly AppDbContext _dbContext;

        public DbContext _DbContext => _dbContext;

        public UnitOfWorksService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IGenericRepository<TEntity> GenericRepository<TEntity>() where TEntity : class
        {
            return new GenericRepository<TEntity>(_dbContext);
        }

        public async Task CompleteAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
