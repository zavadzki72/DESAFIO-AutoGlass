using Produtos.Domain.Model.ViewModels.Suppliers;

namespace Produtos.Domain.Model.ViewModels.Products
{
    public class RegisterProductViewModel : ProductViewModel
    {
        public RegisterSupplierViewModel? Supplier { get; set; }
    }
}
