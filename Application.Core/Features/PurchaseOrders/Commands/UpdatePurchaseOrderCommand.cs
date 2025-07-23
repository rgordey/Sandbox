// Application/Features/PurchaseOrders/Commands/UpdatePurchaseOrderCommand.cs
using Application.Abstractions;
using Application.Common.Interfaces;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.PurchaseOrders.Commands
{
    public sealed class UpdatePurchaseOrderCommand : ICommand<Unit>
    {
        public Guid Id { get; set; }
        public Guid VendorId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public List<PurchaseOrderDetailDto> OrderDetails { get; set; } = new();
    }

    internal sealed class UpdatePurchaseOrderCommandHandler(IAppDbContext context, IMapper mapper) : ICommandHandler<UpdatePurchaseOrderCommand, Unit>
    {
        public async Task<Unit> Handle(UpdatePurchaseOrderCommand request, CancellationToken ct)
        {
            var purchaseOrder = await context.PurchaseOrders
                .AsTracking()
                .Include(po => po.OrderDetails)
                .FirstOrDefaultAsync(po => po.Id == request.Id, ct);

            if (purchaseOrder == null)
            {
                throw new KeyNotFoundException($"PurchaseOrder with ID {request.Id} not found.");
            }

            mapper.Map(request, purchaseOrder);

            // Handle OrderDetails: Remove deleted, update existing, add new
            purchaseOrder.OrderDetails.RemoveAll(od => !request.OrderDetails.Any(rd => rd.Id == od.Id));
            foreach (var detailDto in request.OrderDetails)
            {
                var existingDetail = purchaseOrder.OrderDetails.FirstOrDefault(od => od.Id == detailDto.Id);
                if (existingDetail != null)
                {
                    mapper.Map(detailDto, existingDetail);
                }
                else
                {
                    var newDetail = mapper.Map<PurchaseOrderDetail>(detailDto);
                    newDetail.Id = Guid.NewGuid();
                    newDetail.OrderId = purchaseOrder.Id;
                    purchaseOrder.OrderDetails.Add(newDetail);
                }
            }

            await context.SaveChangesAsync(ct);
            return Unit.Value;
        }
    }
}