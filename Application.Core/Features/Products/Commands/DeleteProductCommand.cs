using Application.Abstractions;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Products.Commands
{
    public sealed record DeleteProductCommand(Guid Id) : ICommand<Unit>;

    internal sealed class DeleteProductCommandHandler(IAppDbContext context) : ICommandHandler<DeleteProductCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await context.Products.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken)
                ?? throw new KeyNotFoundException($"Product with ID {request.Id} not found.");

            context.Products.Remove(product);
            await context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}