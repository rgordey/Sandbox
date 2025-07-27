// Application/Validators/CreateOrderCommandValidator.cs
using Application.Features.Orders.Commands;
using FluentValidation;

namespace Application.Validators
{
    public sealed class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(x => x.CustomerId)
                .NotEqual(Guid.Empty)
                .WithMessage("Customer Id must be valid.");

            RuleFor(x => x.OrderDate)
                .NotEmpty()
                .WithMessage("Order Date is required.");

            RuleFor(x => x.TotalAmount)
                .GreaterThan(0)
                .WithMessage("Total Amount must be greater than 0.");

            // Add validation for OrderDetails if needed
            RuleForEach(x => x.OrderDetails).SetValidator(new OrderDetailDtoValidator());
        }
    }

    // Assuming OrderDetailDtoValidator exists or define it
    public sealed class OrderDetailDtoValidator : AbstractValidator<SalesOrderDetailDto>
    {
        public OrderDetailDtoValidator()
        {
            RuleFor(d => d.ProductId)
                .NotEqual(Guid.Empty)
                .WithMessage("Product Id is required.");

            RuleFor(d => d.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantity must be greater than 0.");

            RuleFor(d => d.UnitPrice)
                .GreaterThan(0)
                .WithMessage("Unit Price must be greater than 0.");
        }
    }
}