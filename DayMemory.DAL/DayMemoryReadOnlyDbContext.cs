using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DayMemory.DAL
{
    public class DayMemoryReadOnlyDbContext : IReadDbContext
    {
        private readonly DayMemoryDbContext _dbContext;

        public DayMemoryReadOnlyDbContext(DayMemoryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<TEntity> GetQuery<TEntity>() where TEntity : class
        {
            return _dbContext.Set<TEntity>().AsTracking();
        }
    }
}