using FluentValidation;

namespace Application.Validators
{
    public sealed class AddressDtoValidator : AbstractValidator<AddressDto?>
    {
        public AddressDtoValidator()
        {
            When(a => a != null, () =>
            {
                RuleFor(a => a!.Line1)
                    .NotEmpty().WithMessage("Street is required.")
                    .MaximumLength(100).WithMessage("Street cannot exceed 100 characters.");

                RuleFor(a => a!.City)
                    .NotEmpty().WithMessage("City is required.")
                    .MaximumLength(50).WithMessage("City cannot exceed 50 characters.");

                RuleFor(a => a!.State)
                    .NotEmpty().WithMessage("State is required.")
                    .MaximumLength(50).WithMessage("State cannot exceed 50 characters.");

                RuleFor(a => a!.ZipCode)
                    .NotEmpty().WithMessage("Zip Code is required.")
                    .MaximumLength(20).WithMessage("Zip Code cannot exceed 20 characters.");
            });
        }
    }
}