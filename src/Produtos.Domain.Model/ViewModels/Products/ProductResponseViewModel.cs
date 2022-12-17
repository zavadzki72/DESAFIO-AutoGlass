using Produtos.Domain.Model.ViewModels.Suppliers;

namespace Produtos.Domain.Model.ViewModels.Products
{
    public class ProductResponseViewModel : ProductViewModel
    {
        public int Id { get; set; }
        public SupplierResponseViewModel? Supplier { get; private set; }

        public void SetSupplier(SupplierResponseViewModel supplierResponseViewModel)
        {
            Supplier = supplierResponseViewModel;
        }
    }
}
