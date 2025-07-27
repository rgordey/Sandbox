using Application.Abstractions;
using Application.Common.Interfaces;
using AutoMapper;
using Domain;

namespace Application.Features.Categories.Commands
{
    public sealed record CreateCategoryCommand(
        string Name,
        Guid? ParentId
    ) : ICommand<Guid>;

    internal sealed class CreateCategoryCommandHandler(IAppDbContext context, IMapper mapper) : ICommandHandler<CreateCategoryCommand, Guid>
    {
        public async Task<Guid> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = mapper.Map<Category>(request);
            category.Id = Guid.NewGuid();

            context.Categories.Add(category);
            await context.SaveChangesAsync(cancellationToken);

            return category.Id;
        }
    }
}
