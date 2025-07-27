using Application.Features.Customers.Commands;
using FluentValidation;

namespace Application.Validators
{
    public sealed class DeleteCustomerCommandValidator : AbstractValidator<DeleteCustomerCommand>
    {
        public DeleteCustomerCommandValidator()
        {
            RuleFor(x => x.CustomerId)
                .NotEqual(Guid.Empty)
                .WithMessage("Customer Id must be valid.");
        }
    }
}