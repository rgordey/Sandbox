// Application/Validators/CreatePurchaseOrderCommandValidator.cs (new)
using Application.Features.PurchaseOrders.Commands;
using FluentValidation;

namespace Application.Validators
{
    public class CreatePurchaseOrderCommandValidator : AbstractValidator<CreatePurchaseOrderCommand>
    {
        public CreatePurchaseOrderCommandValidator()
        {
            RuleFor(x => x.VendorId)
                .NotEqual(Guid.Empty)
                .WithMessage("Vendor Id must be valid.");

            RuleFor(x => x.OrderDate)
                .NotEmpty()
                .WithMessage("Order Date is required.");

            RuleFor(x => x.TotalAmount)
                .GreaterThan(0)
                .WithMessage("Total Amount must be greater than 0.");

            RuleForEach(x => x.OrderDetails).SetValidator(new PurchaseOrderDetailDtoValidator());
        }
    }

    public class PurchaseOrderDetailDtoValidator : AbstractValidator<PurchaseOrderDetailDto>
    {
        public PurchaseOrderDetailDtoValidator()
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