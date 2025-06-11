using FluentValidation;
using Transactions;

namespace Application.Validators
{
    public class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
    {
        public UpdateCustomerCommandValidator()
        {
            RuleFor(x => x.Customer.Id)
                .NotEqual(Guid.Empty)
                .WithMessage("Customer Id must be valid.");

            RuleFor(x => x.Customer.Email)
                .NotEmpty()
                .WithMessage("Email is Required")
                .EmailAddress()
                .WithMessage("A valid email address is required.");
        }
    }
}
