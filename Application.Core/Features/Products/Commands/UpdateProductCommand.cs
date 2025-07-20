using Application.Common.Interfaces;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Products.Commands
{
    public sealed record UpdateProductCommand(
        Guid Id,
        string Name,
        decimal BasePrice,
        List<ProductVendorDto> Vendors
    ) : IRequest;

    internal sealed class UpdateProductCommandHandler(IAppDbContext context, IMapper mapper) : IRequestHandler<UpdateProductCommand>
    {
        public async Task Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await context.Products.AsTracking().Include(p => p.ProductVendors).FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken)
                ?? throw new KeyNotFoundException($"Product with ID {request.Id} not found.");

            mapper.Map(request, product);

            context.ProductVendors.RemoveRange(product.ProductVendors);
            product.ProductVendors = request.Vendors.Select(v => new ProductVendor
            {
                ProductId = product.Id,
                VendorId = v.VendorId,
                VendorPrice = v.VendorPrice,
                StockQuantity = v.StockQuantity
            }).ToList();

            await context.SaveChangesAsync(cancellationToken);
        }
    }
}