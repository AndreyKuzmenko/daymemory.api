using System.Linq;

namespace DayMemory.DAL
{
    public interface IReadDbContext
    {
        IQueryable<TEntity> GetQuery<TEntity>() where TEntity : class;
    }
}