// Application/Features/Products/Queries/GetProductVendorsQuery.cs
using Application.Abstractions;
using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Products.Queries
{
    public sealed class GetProductVendorsQuery : IQuery<List<ProductVendorDto>>
    {
    }

    internal sealed class GetProductVendorsQueryHandler(IAppDbContext context, IMapper mapper) : IQueryHandler<GetProductVendorsQuery, List<ProductVendorDto>>
    {
        public async Task<List<ProductVendorDto>> Handle(GetProductVendorsQuery request, CancellationToken cancellationToken)
        {
            return await context.ProductVendors
                .ProjectTo<ProductVendorDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}