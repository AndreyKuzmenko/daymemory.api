using DayMemory.Core.Interfaces.Repositories;
using DayMemory.Core.Models.Personal;
using DayMemory.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace DayMemory.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DayMemoryDbContext _dbContext;

        public UserRepository(DayMemoryDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task CreateTokenAsync(UserToken token, CancellationToken ct)
        {
            await _dbContext.Set<UserToken>().AddAsync(token);
            await _dbContext.SaveChangesAsync(ct);
        }

        public async Task UpdateTokenAsync(UserToken token, CancellationToken ct)
        {
            await _dbContext.SaveChangesAsync(ct);
        }


        public async Task<UserToken?> GetTokenAsync(string tokenKey, string userId, CancellationToken ct)
        {
            return await _dbContext.Set<UserToken>().FirstOrDefaultAsync(x => x.RefreshToken == tokenKey && x.UserId == userId);
        }
    }

}