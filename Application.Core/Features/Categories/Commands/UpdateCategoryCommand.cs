using Application.Abstractions;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Categories.Commands
{
    public sealed record UpdateCategoryCommand(
        Guid Id,
        string Name,
        Guid? ParentId
    ) : ICommand<Unit>;

    internal sealed class UpdateCategoryCommandHandler(IAppDbContext context, IMapper mapper) : ICommandHandler<UpdateCategoryCommand, Unit>
    {
        public async Task<Unit> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await context.Categories.AsTracking().FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken)
                ?? throw new KeyNotFoundException($"Category with ID {request.Id} not found.");

            mapper.Map(request, category);

            // Prevent circular reference or self-parenting
            if (request.ParentId == request.Id)
            {
                throw new ValidationException("A category cannot be its own parent.");
            }

            await context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
