using Application;
using Application.Features.Customers.Commands;
using AutoMapper;
using Domain;

namespace Application.Mappings
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            
            // Customer mappings
            CreateMap<CreateCustomerCommand, Customer>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Orders, opt => opt.Ignore())
                .ForMember(dest => dest.MailingAddress, opt => opt.MapFrom(src => src.MailingAddress))
                .ForMember(dest => dest.ShippingAddress, opt => opt.MapFrom(src => src.ShippingAddress));

            CreateMap<Customer, CustomerDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FirstName + " " + src.LastName))
                .ReverseMap()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FullName.Split(new[] { ' ' }, 2)[0]))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.FullName.Split(new[] { ' ' }, 2)[1]));
        }
    }
}