using DayMemory.Core.Interfaces.Repositories;

namespace DayMemory.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DayMemoryDbContext _dbContext;

        public UserRepository(DayMemoryDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
    }

}