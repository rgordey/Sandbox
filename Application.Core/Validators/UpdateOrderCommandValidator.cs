// Application/Validators/UpdateOrderCommandValidator.cs
using Application.Features.Orders.Commands;
using FluentValidation;

namespace Application.Validators
{
    public sealed class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
    {
        public UpdateOrderCommandValidator()
        {
            RuleFor(x => x.OrderId)
                .NotEqual(Guid.Empty)
                .WithMessage("Order Id must be valid.");

            RuleFor(x => x.OrderDate)
                .NotEmpty()
                .WithMessage("Order Date is required.");

            RuleFor(x => x.TotalAmount)
                .GreaterThan(0)
                .WithMessage("Total Amount must be greater than 0.");

            RuleForEach(x => x.OrderDetails).SetValidator(new OrderDetailDtoValidator());
        }
    }
}