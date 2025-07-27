using Application.Abstractions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Categories.Commands
{
    public sealed record DeleteCategoryCommand(Guid Id) : ICommand<Unit>;

    internal sealed class DeleteCategoryCommandHandler(IAppDbContext context) : ICommandHandler<DeleteCategoryCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await context.Categories
                .Include(c => c.Children)
                .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (category == null)
            {
                throw new KeyNotFoundException($"Category with ID {request.Id} not found.");
            }

            if (category.Children.Any())
            {
                throw new ValidationException("Cannot delete a category that has subcategories.");
            }

            var hasProducts = await context.Products.AnyAsync(p => p.CategoryId == request.Id, cancellationToken);
            if (hasProducts)
            {
                throw new ValidationException("Cannot delete a category that has associated products.");
            }

            context.Categories.Remove(category);
            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}