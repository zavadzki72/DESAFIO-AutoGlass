using FluentValidation;

namespace Produtos.Domain.Products.Edit
{
    public class EditProductValidator : ProductValidator<EditProductCommand>
    {
        public EditProductValidator()
        {
            ValidateId();
            ValidateDates();

            RuleFor(x => x.SupplierCnpj)
                .Length(14)
                .When(x => !string.IsNullOrWhiteSpace(x.SupplierCnpj))
                .WithMessage("The field SupplierCnpj is invalid");
        }

        protected override void ValidateDates()
        {
            RuleFor(x => x.ValidDate)
                .GreaterThan(x => x.ManufacturingDate)
                .When(x => x.ValidDate.HasValue && x.ManufacturingDate.HasValue)
                .WithMessage("The Valid date cannot be samaller then Manufacturing date");
        }
    }
}
