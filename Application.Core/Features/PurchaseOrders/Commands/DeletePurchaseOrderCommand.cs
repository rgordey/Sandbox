// Application/Features/PurchaseOrders/Commands/DeletePurchaseOrderCommand.cs
using Application.Abstractions;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.PurchaseOrders.Commands
{
    public sealed class DeletePurchaseOrderCommand : ICommand<Guid>
    {
        public Guid Id { get; set; }
    }

    internal sealed class DeletePurchaseOrderCommandHandler(IAppDbContext context) : ICommandHandler<DeletePurchaseOrderCommand, Guid>
    {
        public async Task<Guid> Handle(DeletePurchaseOrderCommand request, CancellationToken ct)
        {
            var purchaseOrder = await context.PurchaseOrders
                .Include(po => po.OrderDetails)
                .FirstOrDefaultAsync(po => po.Id == request.Id, ct);

            if (purchaseOrder == null)
            {
                throw new KeyNotFoundException($"PurchaseOrder with ID {request.Id} not found.");
            }

            context.PurchaseOrders.Remove(purchaseOrder);
            await context.SaveChangesAsync(ct);
            return request.Id;
        }
    }
}