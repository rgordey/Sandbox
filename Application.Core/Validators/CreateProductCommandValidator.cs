using Application.Features.Products.Commands;
using FluentValidation;

namespace Application.Validators
{
    public sealed class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(p => p.Name).NotEmpty().MaximumLength(100);
            RuleFor(p => p.BasePrice).GreaterThanOrEqualTo(0);
            RuleFor(p => p.Weight).GreaterThanOrEqualTo(0);
            RuleFor(p => p.WeightUnit).IsInEnum();
            RuleFor(p => p.Length).GreaterThanOrEqualTo(0);
            RuleFor(p => p.Width).GreaterThanOrEqualTo(0);
            RuleFor(p => p.Height).GreaterThanOrEqualTo(0);
            RuleFor(p => p.DimensionUnit).IsInEnum();
            RuleFor(p => p.CategoryId).NotEmpty();  // Assuming category is required; remove if optional
            RuleForEach(p => p.Vendors).SetValidator(new ProductVendorDtoValidator());
        }
    }
}