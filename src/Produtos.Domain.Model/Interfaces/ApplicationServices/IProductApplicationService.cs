using Produtos.Domain.Model.ViewModels.Products;

namespace Produtos.Domain.Model.Interfaces.ApplicationServices
{
    public interface IProductApplicationService
    {
        Task<ServiceResult<ProductResponseViewModel>> GetById(int id);
        Task<ServiceResult<List<PaginatedProductResponseViewModel>>> GetByFilter(GetProductsByFilter filter);
        Task<ServiceResult<int>> Register(RegisterProductViewModel registerProductViewModel);
        Task<ServiceResult> Edit(int id, RegisterProductViewModel registerProductViewModel);
        Task<ServiceResult> Delete(int id);
    }
}
