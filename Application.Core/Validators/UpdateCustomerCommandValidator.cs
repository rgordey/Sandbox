using Application.Features.Customers.Commands;
using FluentValidation;

namespace Application.Validators
{

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