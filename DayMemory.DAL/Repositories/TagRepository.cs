using DayMemory.Core.Interfaces.Repositories;
using DayMemory.Core.Models.Personal;
using Microsoft.EntityFrameworkCore;

namespace DayMemory.DAL.Repositories
{
    public class TagRepository : Repository<Tag>, ITagRepository
    {
        public TagRepository(DayMemoryDbContext dbContext) : base(dbContext)
        {
        }

        protected override IQueryable<Tag> GetQuery()
        {
            return base.GetQuery();
        }

    }
}