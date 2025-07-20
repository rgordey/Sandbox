using Application.Abstractions;
using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Products.Queries
{
    public sealed record GetProductQuery(Guid Id) : IQuery<ProductDto>;

    internal sealed class GetProductQueryHandler(IAppDbContext context, IMapper mapper) : IQueryHandler<GetProductQuery, ProductDto>
    {
        public async Task<ProductDto> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            return await context.Products
                .Where(p => p.Id == request.Id)
                .ProjectTo<ProductDto>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken)
                ?? throw new KeyNotFoundException($"Product with ID {request.Id} not found.");
        }
    }
}