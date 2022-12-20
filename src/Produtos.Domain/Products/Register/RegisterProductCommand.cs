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
    }
}
