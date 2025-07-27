using Application.Abstractions;
using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;



namespace Application.Features.Categories.Queries
{
    public sealed record GetCategoriesQuery() : IQuery<List<CategoryDto>>;

    internal sealed class GetCategoriesQueryHandler(IAppDbContext context, IMapper mapper) : IQueryHandler<GetCategoriesQuery, List<CategoryDto>>
    {
        public async Task<List<CategoryDto>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            return await context.Categories
                .ProjectTo<CategoryDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}
