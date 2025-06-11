using Application.Core;
using AutoMapper;
using Domain;

namespace Application.Mappings
{
    // AutoMapper Profile    
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Customer, CustomerDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ReverseMap();

            CreateMap<Order, OrderDto>()
                .ReverseMap();

            CreateMap<OrderDetail, OrderDetailDto>()
                .ReverseMap();
        }
    }
}


