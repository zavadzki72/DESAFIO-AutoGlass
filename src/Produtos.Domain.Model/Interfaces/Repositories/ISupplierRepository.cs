using Produtos.Domain.Model.Entities;

namespace Produtos.Domain.Model.Interfaces.Repositories
{
    public interface ISupplierRepository : IBaseRepository<Supplier>
    {
        Task<Supplier?> GetByCnpj(string cnpj);
    }
}
