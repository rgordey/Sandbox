using FluentValidation;

namespace Application.Validators
{
    public sealed class ProductVendorDtoValidator : AbstractValidator<ProductVendorDto>
    {
        public ProductVendorDtoValidator()
        {
            RuleFor(pv => pv.VendorId).NotEmpty();
            RuleFor(pv => pv.VendorPrice).GreaterThanOrEqualTo(0);
            RuleFor(pv => pv.StockQuantity).GreaterThanOrEqualTo(0);
        }
    }
}