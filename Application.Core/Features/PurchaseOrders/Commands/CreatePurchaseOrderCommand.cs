// Application/Features/PurchaseOrders/Commands/CreatePurchaseOrderCommand.cs
using Application.Abstractions;
using Application.Common.Interfaces;
using Domain;
using MediatR;

namespace Application.Features.PurchaseOrders.Commands
{
    public sealed class CreatePurchaseOrderCommand : ICommand<Guid>
    {
        public Guid VendorId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public List<PurchaseOrderDetailDto> OrderDetails { get; set; } = new();
    }

    internal sealed class CreatePurchaseOrderCommandHandler(IAppDbContext context) : ICommandHandler<CreatePurchaseOrderCommand, Guid>
    {
        public async Task<Guid> Handle(CreatePurchaseOrderCommand request, CancellationToken ct)
        {
            var purchaseOrder = new PurchaseOrder
            {
                Id = Guid.NewGuid(),
                VendorId = request.VendorId,
                OrderDate = request.OrderDate,
                TotalAmount = request.TotalAmount
            };

            foreach (var dto in request.OrderDetails)
            {
                purchaseOrder.OrderDetails.Add(new PurchaseOrderDetail
                {
                    Id = Guid.NewGuid(),
                    OrderId = purchaseOrder.Id,
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity,
                    UnitPrice = dto.UnitPrice
                });
            }

            try
            {
                context.PurchaseOrders.Add(purchaseOrder);
                await context.SaveChangesAsync(ct);
                return purchaseOrder.Id;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}