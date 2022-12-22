using FluentValidation;

namespace Produtos.Domain.Model.ViewModels.Products.Validator
{
    public class GetProductsByFilterValidator : AbstractValidator<GetProductsByFilter>
    {
        public GetProductsByFilterValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.Size)
                .GreaterThan(0);

            RuleFor(x => x.MaxManufactureDate)
                .GreaterThanOrEqualTo(x => x.MinManufactureDate)
                .When(x => x.MinManufactureDate.HasValue)
                .WithMessage($"Range of MinManufactureDate | MaxManufactureDate is invalid");

            RuleFor(x => x.MaxValidDate)
                .GreaterThanOrEqualTo(x => x.MinValidDate)
                .When(x => x.MinValidDate.HasValue)
                .WithMessage($"Range of MaxValidDate | MinValidDate is invalid");
        }
    }
}
