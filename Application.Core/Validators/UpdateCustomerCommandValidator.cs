using Application.Features.Customers.Commands;
using FluentValidation;

namespace Application.Validators
{
    public class AddressDtoValidator : AbstractValidator<AddressDto?>
    {
        public AddressDtoValidator()
        {
            When(a => a != null, () =>
            {
                RuleFor(a => a!.Street)
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

    public class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
    {
        public UpdateCustomerCommandValidator()
        {
            RuleFor(x => x.Customer.Id)
                .NotEqual(Guid.Empty)
                .WithMessage("Customer Id must be valid.");

            RuleFor(x => x.Customer.FullName)
                .Must(fullName => fullName.Split(' ').Length == 2)
                .WithMessage("FullName must contain both first and last names separated by a space.");

            RuleFor(x => x.Customer.Email)
                .NotEmpty()
                .WithMessage("Email is Required")
                .EmailAddress()
                .WithMessage("A valid email address is required.");

            RuleFor(x => x.Customer.MailingAddress)
                .SetValidator(new AddressDtoValidator());

            RuleFor(x => x.Customer.ShippingAddress)
                .SetValidator(new AddressDtoValidator());
        }
    }
}