// Application/Features/PurchaseOrders/Commands/CreatePurchaseOrderCommand.cs (new)
using Application.Abstractions;
using Application.Common.Interfaces;
using AutoMapper;
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

    internal sealed class CreatePurchaseOrderCommandHandler(IAppDbContext context, IMapper mapper) : ICommandHandler<CreatePurchaseOrderCommand, Guid>
    {
        public async Task<Guid> Handle(CreatePurchaseOrderCommand request, CancellationToken ct)
        {
            var purchaseOrder = mapper.Map<PurchaseOrder>(request);
            purchaseOrder.Id = Guid.NewGuid();
            context.PurchaseOrders.Add(purchaseOrder);
            await context.SaveChangesAsync(ct);
            return purchaseOrder.Id;
        }
    }
}