using Produtos.Domain.Model.Entities;

namespace Produtos.Domain.Model.Interfaces.Repositories
{
    public interface IBaseRepository<TEntity> 
        where TEntity : BaseEntity
    {
        Task<TEntity?> GetById(int id);
        Task<TEntity?> GetByIdReadOnly(int id);
        Task<List<TEntity>?> GetAll();
        Task<List<TEntity>?> GetAllReadOnly();
        Task<TEntity> Add(TEntity entity);
        Task AddRange(List<TEntity> entity);
        Task Update(TEntity entity);
        Task Delete(TEntity entity);
    }
}
