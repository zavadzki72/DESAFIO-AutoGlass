using Produtos.Domain.Model.ViewModels.Products;

namespace Produtos.Domain.Products.Register
{
    public class RegisterProductCommand : ProductCommand<int>
    {
        public RegisterProductCommand(string description, DateTime manufacturingDate, DateTime validDate, string supplierDescritpion, string supplierCnpj)
        {
            Description = description;
            ManufacturingDate = manufacturingDate;
            ValidDate = validDate;
            SupplierDescritpion = supplierDescritpion;
            SupplierCnpj = supplierCnpj;
        }

        public string SupplierDescritpion { get; set; }
        public string SupplierCnpj { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new RegisterProductValidator().Validate(this);
            return ValidationResult.IsValid;
        }

        public static RegisterProductCommand CreateByRegisterViewModel(RegisterProductViewModel registerProductViewModel)
        {
            return new RegisterProductCommand(
                registerProductViewModel.Description, 
                registerProductViewModel.ManufacturingDate.Value, 
                registerProductViewModel.ValidDate.Value, 
                registerProductViewModel.Supplier.Description, 
                registerProductViewModel.Supplier.Cnpj
            );
        }
    }
}
