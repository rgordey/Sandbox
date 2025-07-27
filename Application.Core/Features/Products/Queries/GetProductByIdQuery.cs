using Application.Abstractions;
using Application.Common.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Products.Queries
{
    public sealed record GetProductByIdQuery(Guid Id) : IQuery<ProductDto>;

    internal sealed class GetProductByIdQueryHandler(IAppDbContext context, IMapper mapper) : IQueryHandler<GetProductByIdQuery, ProductDto>
    {
        public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await context.Products
                .Include(p => p.ProductVendors)
                .ThenInclude(pv => pv.Vendor!)
                .ThenInclude(v => v.Address)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (product == null)
            {
                throw new KeyNotFoundException($"Product with ID {request.Id} not found.");
            }

            // Load category hierarchy recursively
            var category = product.Category;
            while (category != null && category.ParentId.HasValue)
            {
                category.Parent = await context.Categories.FindAsync(category.ParentId);
                category = category.Parent;
            }

            return mapper.Map<ProductDto>(product);
        }
    }
}