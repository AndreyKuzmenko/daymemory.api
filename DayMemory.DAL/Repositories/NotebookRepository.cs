using DayMemory.Core.Interfaces.Repositories;
using DayMemory.Core.Models.Personal;
using Microsoft.EntityFrameworkCore;

namespace DayMemory.DAL.Repositories
{
    public class NotebookRepository : Repository<Notebook>, INotebookRepository
    {
        public NotebookRepository(DayMemoryDbContext dbContext) : base(dbContext)
        {
        }

        protected override IQueryable<Notebook> GetQuery()
        {
            return base.GetQuery();
        }

    }
}