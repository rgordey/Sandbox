using AutoMapper;
using Domain;

namespace Application.Mappings
{
    public class RetailPriceResolver : IValueResolver<Product, ProductDto, decimal>
    {
        public decimal Resolve(Product source, ProductDto destination, decimal destMember, ResolutionContext context)
        {
            return source.ProductVendors?.Any() == true ? source.ProductVendors.Min(pv => pv.VendorPrice) * 2m : source.BasePrice;
        }
    }
}
