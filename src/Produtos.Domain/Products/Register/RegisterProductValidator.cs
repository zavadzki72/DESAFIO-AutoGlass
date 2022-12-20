using FluentValidation;

namespace Produtos.Domain.Products.Register
{
    public class RegisterProductValidator : ProductValidator<RegisterProductCommand, int>
    {
        public RegisterProductValidator()
        {
            ValidateDescription();
            ValidateDates();

            RuleFor(x => x.SupplierDescritpion)
                .NotEmpty()
                .WithMessage("The field SupplierDescritpion is required");

            RuleFor(x => x.SupplierCnpj)
                .NotEmpty()
                .Length(14)
                .WithMessage("The field SupplierCnpj is invalid");
        }
    }
}
