using Application.Features.Vendors.Commands;
using FluentValidation;

namespace Application.Validators
{
    public sealed class CreateVendorCommandValidator : AbstractValidator<CreateVendorCommand>
    {
        public CreateVendorCommandValidator()
        {
            RuleFor(v => v.Name).NotEmpty().MaximumLength(100);
            RuleFor(v => v.ContactEmail).NotEmpty().EmailAddress();
            RuleFor(v => v.Address).NotNull().SetValidator(new AddressDtoValidator());
        }
    }
}