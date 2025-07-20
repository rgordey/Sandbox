using Application.Features.Products.Commands;
using FluentValidation;

namespace Application.Validators
{
    public sealed class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(p => p.Id).NotEmpty();
            RuleFor(p => p.Name).NotEmpty().MaximumLength(100);
            RuleFor(p => p.BasePrice).GreaterThanOrEqualTo(0);
            RuleForEach(p => p.Vendors).SetValidator(new ProductVendorDtoValidator());
        }
    }
}