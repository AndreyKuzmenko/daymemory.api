using System.Threading;
using DayMemory.Core.Models.Common;
using System.Threading.Tasks;

namespace DayMemory.Core.Interfaces.Repositories
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        Task<TEntity?> LoadByIdAsync(string id);
        Task<TEntity?> LoadByIdAsync(string id, CancellationToken ct);

        Task<bool> ExistsAsync(string id);
        Task<bool> ExistsAsync(string id, CancellationToken ct);

        Task CreateAsync(TEntity entity);
        Task CreateAsync(TEntity entity, CancellationToken ct);

        Task UpdateAsync(TEntity entity);
        Task UpdateAsync(TEntity entity, CancellationToken ct);


        Task DeleteAsync(TEntity entity);
        Task DeleteAsync(TEntity entity, CancellationToken ct);

        Task DeleteByIdAsync(string id);
        Task DeleteByIdAsync(string id, CancellationToken ct);
    }
}
