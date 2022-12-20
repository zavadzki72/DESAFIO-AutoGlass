using FluentValidation;

namespace Produtos.Domain.Model.ViewModels.Products.Validator
{
    public class GetProductsByFilterValidator : AbstractValidator<GetProductsByFilter>
    {
        public GetProductsByFilterValidator()
        {
            RuleFor(x => x.MaxManufactureDate)
                .NotNull()
                .GreaterThanOrEqualTo(x => x.MinManufactureDate)
                .When(x => x.MinManufactureDate.HasValue)
                .WithMessage($"Range of MinManufactureDate | MaxManufactureDate is invalid");

            RuleFor(x => x.MinManufactureDate)
                .NotNull()
                .When(x => x.MaxManufactureDate.HasValue)
                .WithMessage($"Range of MinManufactureDate | MaxManufactureDate is invalid");

            RuleFor(x => x.MaxValidDate)
                .NotNull()
                .GreaterThanOrEqualTo(x => x.MinValidDate)
                .When(x => x.MinValidDate.HasValue)
                .WithMessage($"Range of MaxValidDate | MinValidDate is invalid");

            RuleFor(x => x.MinValidDate)
                .NotNull()
                .When(x => x.MaxValidDate.HasValue)
                .WithMessage($"Range of MaxValidDate | MinValidDate is invalid");
        }
    }
}
