using DayMemory.Core.Interfaces.Repositories;
using DayMemory.Core.Models.Common;
using DayMemory.Core.Models.Personal;

namespace DayMemory.DAL.Repositories
{
    public class FileRepository : Repository<Core.Models.Personal.File>, IFileRepository
    {
        public FileRepository(DayMemoryDbContext dbContext) : base(dbContext)
        {
        }
    }
}