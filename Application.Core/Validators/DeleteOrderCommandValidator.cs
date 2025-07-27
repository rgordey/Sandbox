// Application/Validators/DeleteOrderCommandValidator.cs
using Application.Features.Orders.Commands;
using FluentValidation;

namespace Application.Validators
{
    public sealed class DeleteOrderCommandValidator : AbstractValidator<DeleteOrderCommand>
    {
        public DeleteOrderCommandValidator()
        {
            RuleFor(x => x.OrderId)
                .NotEqual(Guid.Empty)
                .WithMessage("Order Id must be valid.");
        }
    }
}