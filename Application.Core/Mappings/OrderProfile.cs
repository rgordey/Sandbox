using Application.Features.Orders.Commands;
using Application.Features.PurchaseOrders.Commands;
using AutoMapper;
using Domain;

namespace Application.Mappings
{
    public sealed class OrderProfile : Profile
    {
        public OrderProfile()
        {
            // Sale Order mappings
            CreateMap<CreateOrderCommand, SalesOrder>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
                .ForMember(dest => dest.SequentialNumber, opt => opt.Ignore())
                .ForMember(dest => dest.OrderNumber, opt => opt.Ignore())
                .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => src.OrderDate))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount))
                .ForMember(dest => dest.OrderDetails, opt => opt.MapFrom(src => src.OrderDetails))
                .ForMember(dest => dest.Status, opt => opt.Ignore())  // Set in handler
                .ForMember(dest => dest.Customer, opt => opt.Ignore());
                

            CreateMap<SalesOrder, SalesOrderDto>()
                .ForMember(dest => dest.CustomerFullName, opt => opt.MapFrom(src => src.Customer != null ? src.Customer.Name : string.Empty))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))  // New: Map order status
                .ForMember(dest => dest.OrderNumber, opt => opt.MapFrom(src => src.OrderNumber))
                .ReverseMap();

            CreateMap<SalesOrderDetail, SalesOrderDetailDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : string.Empty))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))  // New: Map detail status
                .ReverseMap()
                .ForMember(dest => dest.Product, opt => opt.Ignore())
                .ForMember(dest => dest.Order, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore());  // Set in handler

            // Purchase Order mappings
            CreateMap<CreatePurchaseOrderCommand, PurchaseOrder>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // Id is set manually in the handler
                .ForMember(dest => dest.OrderDetails, opt => opt.MapFrom(src => src.OrderDetails))
                .ForMember(dest => dest.Vendor, opt => opt.Ignore());

            CreateMap<UpdatePurchaseOrderCommand, PurchaseOrder>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.OrderDetails, opt => opt.MapFrom(src => src.OrderDetails))
                .ForMember(dest => dest.Vendor, opt => opt.Ignore());

            CreateMap<PurchaseOrder, PurchaseOrderDto>().ReverseMap();

            CreateMap<PurchaseOrder, PurchaseOrderListDto>();

            CreateMap<PurchaseOrderDetail, PurchaseOrderDetailDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : string.Empty))
                .ReverseMap()
                .ForMember(dest => dest.Product, opt => opt.Ignore())
                .ForMember(dest => dest.Order, opt => opt.Ignore());
        }
    }
}