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
            RuleForEach(p => p.Vendors).SetValidator(new ProductVendorDtoValidator());
        }
    }
}