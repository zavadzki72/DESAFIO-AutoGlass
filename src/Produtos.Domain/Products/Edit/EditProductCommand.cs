namespace Produtos.Domain.Products.Edit
{
    public class EditProductCommand : ProductCommand
    {
        public EditProductCommand(int id, string? description, DateTime? manufacturingDate, DateTime? validDate, string? supplierDescritpion, string? supplierCnpj)
        {
            Id = id;
            Description = description;
            ManufacturingDate = manufacturingDate;
            ValidDate = validDate;
            SupplierDescritpion = supplierDescritpion;
            SupplierCnpj = supplierCnpj;
        }

        public string? SupplierDescritpion { get; set; }
        public string? SupplierCnpj { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new EditProductValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
