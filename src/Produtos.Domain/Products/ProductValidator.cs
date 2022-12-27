using FluentValidation;
using Produtos.Domain.Model.Entities;

namespace Produtos.Domain.Products
{
    public abstract class ProductValidator<T> : AbstractValidator<T> where T : ProductCommand
    {
        protected const string REQUIRED_FIELD = "The field {0} is required";

        protected void ValidateId()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage(string.Format(REQUIRED_FIELD, nameof(Product.Id)));
        }

        protected void ValidateSupplierId()
        {
            RuleFor(x => x.SupplierId)
                .NotEmpty()
                .WithMessage(string.Format(REQUIRED_FIELD, nameof(Product.SupplierId)));
        }

        protected void ValidateDescription()
        {
            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage(string.Format(REQUIRED_FIELD, nameof(Product.Description)));
        }

        protected virtual void ValidateDates()
        {
            RuleFor(x => x.ManufacturingDate)
                .NotEmpty()
                .WithMessage(string.Format(REQUIRED_FIELD, nameof(Product.ManufacturingDate)));

            RuleFor(x => x.ValidDate)
                .NotEmpty()
                .WithMessage(string.Format(REQUIRED_FIELD, nameof(Product.ManufacturingDate)));

            RuleFor(x => x.ValidDate)
                .GreaterThan(x => x.ManufacturingDate)
                .WithMessage("The Manufacturing date cannot be samaller then Valid date");
        }
    }

    public abstract class ProductValidator<T, TResponse> : AbstractValidator<T> where T : ProductCommand<TResponse>
    {
        private const string REQUIRED_FIELD = "The field {0} is required";

        protected void ValidateId()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage(string.Format(REQUIRED_FIELD, nameof(Product.Id)));
        }

        protected void ValidateSupplierId()
        {
            RuleFor(x => x.SupplierId)
                .NotEmpty()
                .WithMessage(string.Format(REQUIRED_FIELD, nameof(Product.SupplierId)));
        }

        protected void ValidateDescription()
        {
            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage(string.Format(REQUIRED_FIELD, nameof(Product.Description)));
        }

        protected void ValidateDates()
        {
            RuleFor(x => x.ManufacturingDate)
                .NotEmpty()
                .WithMessage(string.Format(REQUIRED_FIELD, nameof(Product.ManufacturingDate)));

            RuleFor(x => x.ValidDate)
                .NotEmpty()
                .WithMessage(string.Format(REQUIRED_FIELD, nameof(Product.ManufacturingDate)));

            RuleFor(x => x.ValidDate)
                .GreaterThan(x => x.ManufacturingDate)
                .WithMessage("The Manufacturing date cannot be samaller then Valid date");
        }
    }
}
