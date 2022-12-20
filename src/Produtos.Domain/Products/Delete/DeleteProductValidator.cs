using FluentValidation;

namespace Produtos.Domain.Products.Delete
{
    public class DeleteProductValidator : ProductValidator<DeleteProductCommand>
    {
        public DeleteProductValidator()
        {
            ValidateId();
        }
    }
}
