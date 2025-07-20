// Updated Application/Features/Orders/Commands/CreateOrderCommand.cs
using Application.Abstractions;
using Application.Common.Interfaces;
using AutoMapper;
using Domain;
using MediatR;

namespace Application.Features.Orders.Commands
{
    public sealed class CreateOrderCommand : ICommand<Guid>
    {
        public Guid CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public List<SalesOrderDetailDto> OrderDetails { get; set; } = new();
    }

    internal sealed class CreateOrderCommandHandler(IAppDbContext context, IMapper mapper) : ICommandHandler<CreateOrderCommand, Guid>
    {
        public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken ct)
        {

            var order = mapper.Map<SalesOrder>(request);
            order.Id = Guid.NewGuid();
            foreach (var detail in order.OrderDetails)
            {
                detail.Id = Guid.NewGuid();
                detail.OrderId = order.Id;
                detail.Product = null;
                // Assume UnitPrice comes from request; in real, fetch from Product
            }
            context.Orders.Add(order);
            await context.SaveChangesAsync(ct);
            return order.Id;
        }
    }
}