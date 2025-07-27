using Application;
using Application.Features.Vendors.Commands;
using AutoMapper;
using Domain;

namespace Application.Mappings
{
    public sealed class VendorProfile : Profile
    {
        public VendorProfile()
        {
            // Vendor mappings
            CreateMap<CreateVendorCommand, Vendor>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.PurchaseOrders, opt => opt.Ignore())
                .ForMember(dest => dest.SequentialNumber, opt => opt.Ignore())
                .ForMember(dest => dest.VendorNumber, opt => opt.Ignore())
                .ForMember(dest => dest.ProductVendors, opt => opt.Ignore());

            CreateMap<UpdateVendorCommand, Vendor>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.PurchaseOrders, opt => opt.Ignore())
                .ForMember(dest => dest.SequentialNumber, opt => opt.Ignore())
                .ForMember(dest => dest.VendorNumber, opt => opt.Ignore())
                .ForMember(dest => dest.ProductVendors, opt => opt.Ignore());

            CreateMap<Vendor, VendorDto>()
                .ForMember(dest => dest.PurchaseOrders, opt => opt.Ignore())
                .ForMember(dest => dest.VendorPrice, opt => opt.Ignore())
                .ForMember(dest => dest.StockQuantity, opt => opt.Ignore())
                .ForMember(dest => dest.VendorNumber, opt => opt.MapFrom(src => src.VendorNumber))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address ?? new Address()));

            CreateMap<Vendor, VendorListDto>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address ?? new Address()));
        }
    }
}