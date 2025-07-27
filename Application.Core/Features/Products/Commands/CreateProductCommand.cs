using Application.Abstractions;
using Application.Common.Interfaces;
using AutoMapper;
using Domain;

namespace Application.Features.Products.Commands
{
    public sealed record CreateProductCommand(
        string Name,
        decimal BasePrice,
        decimal Weight,
        WeightUnit WeightUnit,
        decimal Length,
        decimal Width,
        decimal Height,
        DimensionUnit DimensionUnit,
        Guid? CategoryId,
        List<ProductVendorDto> Vendors
    ) : ICommand<Guid>;

    internal sealed class CreateProductCommandHandler(IAppDbContext context, IMapper mapper) : ICommandHandler<CreateProductCommand, Guid>
    {
        public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = mapper.Map<Product>(request);
            product.Id = Guid.NewGuid();

            product.ProductVendors = request.Vendors.Select(v => new ProductVendor
            {
                ProductId = product.Id,
                VendorId = v.VendorId,
                VendorPrice = v.VendorPrice,
                StockQuantity = v.StockQuantity
            }).ToList();

            context.Products.Add(product);
            await context.SaveChangesAsync(cancellationToken);

            return product.Id;
        }
    }
}
