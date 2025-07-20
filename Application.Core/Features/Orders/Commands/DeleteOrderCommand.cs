// Application/Features/Orders/Commands/DeleteOrderCommand.cs
using Application.Abstractions;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Orders.Commands
{
    public sealed class DeleteOrderCommand : ICommand<Unit>
    {
        public Guid OrderId { get; set; }
    }

    internal sealed class DeleteOrderCommandHandler(IAppDbContext context) : ICommandHandler<DeleteOrderCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken ct)
        {
            var order = await context.Orders
                .FirstOrDefaultAsync(o => o.Id == request.OrderId, ct);

            if (order == null)
            {
                throw new Exception("Order not found");
            }

            context.Orders.Remove(order);
            await context.SaveChangesAsync(ct);
            return Unit.Value;
        }
    }
}