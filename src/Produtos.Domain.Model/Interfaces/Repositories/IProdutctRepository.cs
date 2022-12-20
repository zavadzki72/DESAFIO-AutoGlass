using Produtos.Domain.Model.Dtos.Filters;
using Produtos.Domain.Model.Entities;
using Produtos.Domain.Model.ViewModels.Products;

namespace Produtos.Domain.Model.Interfaces.Repositories
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        Task<PagedRepositoryResponse<PaginatedProductResponseViewModel>> GetByFilter(ProductFilter filter);
    }
}
