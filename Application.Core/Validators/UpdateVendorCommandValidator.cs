using Application.Features.Vendors.Commands;
using FluentValidation;

namespace Application.Validators
{
    public sealed class UpdateVendorCommandValidator : AbstractValidator<UpdateVendorCommand>
    {
        public UpdateVendorCommandValidator()
        {
            RuleFor(v => v.Id).NotEmpty();
            RuleFor(v => v.Name).NotEmpty().MaximumLength(100);
            RuleFor(v => v.ContactEmail).NotEmpty().EmailAddress();
            RuleFor(v => v.Address).NotNull().SetValidator(new AddressDtoValidator());
        }
    }
}