// Application/Features/Orders/Commands/UpdateOrderCommand.cs
using Application.Abstractions;
using Application.Common.Interfaces;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Orders.Commands
{
    public sealed class UpdateOrderCommand : ICommand<Unit>
    {
        public Guid OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public List<SalesOrderDetailDto> OrderDetails { get; set; } = new();
    }

    internal sealed class UpdateOrderCommandHandler(IAppDbContext context, IMapper mapper) : ICommandHandler<UpdateOrderCommand, Unit>
    {
        public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken ct)
        {
            var order = await context.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.Id == request.OrderId, ct);

            if (order == null)
            {
                throw new Exception("Order not found");
            }

            // Validate ProductIds and set UnitPrice
            var productIds = request.OrderDetails.Select(d => d.ProductId).ToList();
            var products = await context.Products
                .Where(p => productIds.Contains(p.Id))
                .ToDictionaryAsync(p => p.Id, p => p, ct);

            foreach (var detail in request.OrderDetails)
            {
                if (!products.ContainsKey(detail.ProductId))
                {
                    throw new Exception($"Product with Id {detail.ProductId} not found");
                }
                detail.UnitPrice = products[detail.ProductId].BasePrice;
            }

            mapper.Map(request, order);
            context.OrderDetails.RemoveRange(order.OrderDetails);
            order.OrderDetails = mapper.Map<List<SalesOrderDetail>>(request.OrderDetails);
            foreach (var detail in order.OrderDetails)
            {
                detail.OrderId = order.Id;
                detail.Product = null; // Prevent attaching Product entity
            }

            await context.SaveChangesAsync(ct);
            return Unit.Value;
        }
    }
}