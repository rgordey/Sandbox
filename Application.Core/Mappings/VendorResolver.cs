using Application.Features.Products.Commands;
using AutoMapper;
using Domain;

namespace Application.Mappings
{
    public class VendorResolver : IValueResolver<Product, ProductDto, List<VendorDto>>
    {
        public List<VendorDto> Resolve(Product source, ProductDto destination, List<VendorDto> destMember, ResolutionContext context)
        {
            if (source.ProductVendors == null)
            {
                return new List<VendorDto>();
            }

            return source.ProductVendors.Select(pv => new VendorDto
            {
                Id = pv.Vendor?.Id ?? Guid.Empty,
                Name = pv.Vendor?.Name ?? "Unknown Vendor",
                ContactEmail = pv.Vendor?.ContactEmail ?? string.Empty,
                Address = pv.Vendor?.Address != null
                    ? new AddressDto
                    {
                        Line1 = pv.Vendor.Address.Line1 ?? string.Empty,
                        City = pv.Vendor.Address.City ?? string.Empty,
                        State = pv.Vendor.Address.State ?? string.Empty,
                        ZipCode = pv.Vendor.Address.ZipCode ?? string.Empty
                    }
                    : new AddressDto(),
                VendorPrice = pv.VendorPrice,
                StockQuantity = pv.StockQuantity
            }).ToList();
        }
    }
}