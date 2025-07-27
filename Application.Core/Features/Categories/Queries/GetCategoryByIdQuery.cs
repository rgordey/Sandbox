using Application.Abstractions;
using Application.Common.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Categories.Queries
{
    public sealed record GetCategoryByIdQuery(Guid Id) : IQuery<CategoryDto>;

    internal sealed class GetCategoryByIdQueryHandler(IAppDbContext context, IMapper mapper) : IQueryHandler<GetCategoryByIdQuery, CategoryDto>
    {
        public async Task<CategoryDto> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var category = await context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (category == null)
            {
                throw new KeyNotFoundException($"Category with ID {request.Id} not found.");
            }

            return mapper.Map<CategoryDto>(category);
        }
    }
}