using Application.Features.Products.Commands;
using AutoMapper;
using Domain;

namespace Application.Mappings
{
    public sealed class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<CreateProductCommand, Product>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ProductVendors, opt => opt.Ignore());

            CreateMap<UpdateProductCommand, Product>()
                .ForMember(dest => dest.ProductVendors, opt => opt.Ignore());

            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.Vendors, opt => opt.MapFrom(src => src.ProductVendors.Select(pv => new VendorDto
                {
                    Id = pv.Vendor.Id,
                    Name = pv.Vendor.Name,
                    ContactEmail = pv.Vendor.ContactEmail,
                    Address = new AddressDto
                    {
                        Line1 = pv.Vendor.Address.Line1,
                        City = pv.Vendor.Address.City,
                        State = pv.Vendor.Address.State,
                        ZipCode = pv.Vendor.Address.ZipCode
                    },
                    VendorPrice = pv.VendorPrice,
                    StockQuantity = pv.StockQuantity
                })))
                .ForMember(dest => dest.RetailPrice, opt => opt.MapFrom(src => src.ProductVendors.Any() ? src.ProductVendors.Min(pv => pv.VendorPrice) * 2m : src.BasePrice))
                .ForMember(dest => dest.CategoryPath, opt => opt.MapFrom(src => GetCategoryPath(src.Category)));

            CreateMap<ProductVendor, ProductVendorDto>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.VendorId, opt => opt.MapFrom(src => src.VendorId));
        }

        private static string GetCategoryPath(Category? category)
        {
            if (category == null) return string.Empty;

            var path = new List<string> { category.Name };
            var parent = category.Parent;
            while (parent != null)
            {
                path.Insert(0, parent.Name);
                parent = parent.Parent;
            }
            return string.Join(" > ", path);
        }
    }
}