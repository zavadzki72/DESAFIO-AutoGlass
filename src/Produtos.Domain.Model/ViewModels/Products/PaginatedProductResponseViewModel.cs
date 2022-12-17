using Produtos.Domain.Model.ApiContracts;
using Produtos.Domain.Model.ViewModels.Suppliers;

namespace Produtos.Domain.Model.ViewModels.Products
{
    public class PaginatedProductResponseViewModel : PaginatedOrderedResult
    {
        public PaginatedProductResponseViewModel() : base(1) { }
        public PaginatedProductResponseViewModel(long defaultOrderId) : base(defaultOrderId) { }

        public int Id { get; set; }
        public string? Description { get; set; }
        public DateTime ManufacturingDate { get; set; }
        public DateTime ValidDate { get; set; }
        public SupplierResponseViewModel? Supplier { get; private set; }

        public void SetSupplier(SupplierResponseViewModel supplierResponseViewModel)
        {
            Supplier = supplierResponseViewModel;
        }
    }
}
