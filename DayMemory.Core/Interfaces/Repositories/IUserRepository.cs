using DayMemory.Core.Models.Common;
using DayMemory.Core.Models.Personal;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DayMemory.Core.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<UserToken?> GetTokenAsync(string tokenKey, string userId, CancellationToken ct);

        Task CreateTokenAsync(UserToken token, CancellationToken ct);

        Task UpdateTokenAsync(UserToken token, CancellationToken ct);
    }
}