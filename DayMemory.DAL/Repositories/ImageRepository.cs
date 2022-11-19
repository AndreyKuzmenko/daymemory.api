using DayMemory.Core.Interfaces.Repositories;
using DayMemory.Core.Models.Common;
using DayMemory.Core.Models.Personal;

namespace DayMemory.DAL.Repositories
{
    public class ImageRepository : Repository<Image>, IImageRepository
    {
        public ImageRepository(DayMemoryDbContext dbContext) : base(dbContext)
        {
        }
    }
}