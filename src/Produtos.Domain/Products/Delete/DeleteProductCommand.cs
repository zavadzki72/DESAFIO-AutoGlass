namespace Produtos.Domain.Products.Delete
{
    public class DeleteProductCommand : ProductCommand
    {
        public DeleteProductCommand(int id)
        {
            Id = id;
        }

        public override bool IsValid()
        {
            ValidationResult = new DeleteProductValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
