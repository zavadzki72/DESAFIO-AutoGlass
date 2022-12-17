using Microsoft.EntityFrameworkCore;
using Produtos.Domain.Model.Entities;
using Produtos.Domain.Model.Interfaces.Repositories;

namespace Produtos.Infra.SqlServer.Repositories
{
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity>
        where TEntity : BaseEntity
    {

        public readonly ApplicationDbContext _applicationDbContext;

        public BaseRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public virtual async Task<TEntity?> GetById(int id)
        {
            return await _applicationDbContext.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == id);
        }

        public virtual async Task<List<TEntity>?> GetAll()
        {
            return await _applicationDbContext.Set<TEntity>().ToListAsync();
        }

        public virtual async Task<TEntity> Add(TEntity entity)
        {
            var result = await _applicationDbContext.Set<TEntity>().AddAsync(entity);
            return result.Entity;
        }

        public virtual async Task AddRange(List<TEntity> entities)
        {
            await _applicationDbContext.Set<TEntity>().AddRangeAsync(entities);
        }

        public virtual Task Update(TEntity entity)
        {
            _applicationDbContext.Set<TEntity>().Update(entity);
            return Task.CompletedTask;
        }

        public virtual Task Delete(TEntity entity)
        {
            _applicationDbContext.Set<TEntity>().Remove(entity);
            return Task.CompletedTask;
        }
    }
}
