using Application.Features.Customers.Commands;
using Application.Features.Orders.Commands;
using AutoMapper;
using Domain;

namespace Application.Mappings
{
    // AutoMapper Profile    
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateCustomerCommand, Customer>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Orders, opt => opt.Ignore())
                .ForMember(dest => dest.MailingAddress, opt => opt.MapFrom(src => src.MailingAddress))
                .ForMember(dest => dest.ShippingAddress, opt => opt.MapFrom(src => src.ShippingAddress));

            CreateMap<Address, AddressDto>()
                .ReverseMap();

            CreateMap<Customer, CustomerDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ForMember(dest => dest.MailingAddress, opt => opt.MapFrom(src => src.MailingAddress))
                .ForMember(dest => dest.ShippingAddress, opt => opt.MapFrom(src => src.ShippingAddress))
                .ReverseMap()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FullName.Split(new[] { ' ' }, 2)[0]))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.FullName.Split(new[] { ' ' }, 2)[1]))
                .ForMember(dest => dest.MailingAddress, opt => opt.MapFrom(src => src.MailingAddress))
                .ForMember(dest => dest.ShippingAddress, opt => opt.MapFrom(src => src.ShippingAddress));

            CreateMap<CreateOrderCommand, Order>()
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
                .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => src.OrderDate))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount))
                .ForMember(dest => dest.OrderDetails, opt => opt.MapFrom(src => src.OrderDetails));

            CreateMap<UpdateOrderCommand, Order>()
                .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => src.OrderDate))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount))
                .ForMember(dest => dest.OrderDetails, opt => opt.MapFrom(src => src.OrderDetails));

            // Add to Application/Mappings/MappingProfile.cs (merge with existing)
            CreateMap<Product, ProductDto>()
                .ReverseMap();

            CreateMap<OrderDetail, OrderDetailDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : null))
                .ReverseMap()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.Product, opt => opt.Ignore()); // Ignore navigation in reverse

            // Add to Application/Mappings/MappingProfile.cs (merge with existing)
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.CustomerFullName, opt => opt.MapFrom(src => src.Customer != null ? src.Customer.FirstName + " " + src.Customer.LastName : string.Empty));

            
        }
    }
}


