using DayMemory.Core.Interfaces.Repositories;
using DayMemory.Core.Models.Common;
using DayMemory.Core.Models.Personal;

namespace DayMemory.DAL.Repositories
{
    public class LocationRepository : Repository<Location>, ILocationRepository
    {
        public LocationRepository(DayMemoryDbContext dbContext) : base(dbContext)
        {
        }
    }
}