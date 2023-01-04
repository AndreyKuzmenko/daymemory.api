using DayMemory.Core.Interfaces.Repositories;
using DayMemory.Core.Models.Personal;
using Microsoft.EntityFrameworkCore;

namespace DayMemory.DAL.Repositories
{
    public class NoteItemRepository : Repository<NoteItem>, INoteItemRepository
    {
        public NoteItemRepository(DayMemoryDbContext dbContext) : base(dbContext)
        {
        }

        protected override IQueryable<NoteItem> GetQuery()
        {
            return base.GetQuery()
                .Include(i => i.Location)
                .Include(i => i.Files).ThenInclude(x => x.File);
        }

    }
}