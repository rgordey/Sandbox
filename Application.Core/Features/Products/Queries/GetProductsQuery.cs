// Application/Features/Products/Queries/GetProductsQuery.cs
using Application.Abstractions;
using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Products.Queries
{
    public sealed class GetProductsQuery : IQuery<List<ProductDto>>
    {
    }

    internal sealed class GetProductsQueryHandler(IAppDbContext context, IMapper mapper) : IQueryHandler<GetProductsQuery, List<ProductDto>>
    {
        public async Task<List<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            return await context.Products
                .ProjectTo<ProductDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}