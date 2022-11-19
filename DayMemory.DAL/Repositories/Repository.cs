using DayMemory.Core.Interfaces.Repositories;
using DayMemory.Core.Models.Common;
using DayMemory.Core.Models.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DayMemory.DAL.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        protected DbContext DbContext { get; }

        public Repository(DayMemoryDbContext dbContext)
        {
            DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public Task<TEntity?> LoadByIdAsync(string id)
        {
            return LoadByIdAsync(id, CancellationToken.None);
        }

        public async Task<TEntity?> LoadByIdAsync(string id, CancellationToken ct)
        {
            var entity = await GetQuery().FirstOrDefaultAsync(e => e.Id == id, ct);

            return entity;
        }

        public Task<bool> ExistsAsync(string id)
        {
            return ExistsAsync(id, CancellationToken.None);
        }

        public Task<bool> ExistsAsync(string id, CancellationToken ct)
        {
            return DbContext.Set<TEntity>().AnyAsync(e => e.Id == id, ct);
        }

        public Task CreateAsync(TEntity entity)
        {
            return CreateAsync(entity, CancellationToken.None);
        }

        public async Task CreateAsync(TEntity entity, CancellationToken ct)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            DbContext.Set<TEntity>().Add(entity);
            await DbContext.SaveChangesAsync(ct);
        }

        public virtual Task UpdateAsync(TEntity entity)
        {
            return UpdateAsync(entity, CancellationToken.None);
        }

        public virtual async Task UpdateAsync(TEntity entity, CancellationToken ct)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await DbContext.SaveChangesAsync(ct);
        }

        public Task DeleteByIdAsync(string id)
        {
            return DeleteByIdAsync(id, CancellationToken.None);
        }

        public async Task DeleteByIdAsync(string id, CancellationToken ct)
        {
            var entity = await DbContext.Set<TEntity>().FindAsync(id);
            if (entity == null)
            {
                return;
            }

            DbContext.Set<TEntity>().Attach(entity);
            DbContext.Set<TEntity>().Remove(entity);

            await DbContext.SaveChangesAsync(ct);
        }

        protected virtual IQueryable<TEntity> GetQuery()
        {
            return DbContext.Set<TEntity>();
        }

        public async Task DeleteAsync(TEntity entity)
        {
            DbContext.Set<TEntity>().Attach(entity);
            DbContext.Set<TEntity>().Remove(entity);
            await DbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(TEntity entity, CancellationToken ct)
        {
            DbContext.Set<TEntity>().Attach(entity);
            DbContext.Set<TEntity>().Remove(entity);
            await DbContext.SaveChangesAsync(ct);
        }
    }
}
