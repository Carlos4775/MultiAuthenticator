
using UnitOfWork.GenericRepository;

namespace UnitOfWork.UnitOfWork
{
    public interface IUnitOfWorksService
    {
        IGenericRepository<TEntity> GenericRepository<TEntity>() where TEntity : class;
        Task CompleteAsync();
    }
}
