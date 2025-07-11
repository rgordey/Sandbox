using AutoMapper;
using Domain;

namespace Application.Core.Mappings
{
    // AutoMapper Profile    
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Customer, CustomerDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ReverseMap()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FullName.Split(new[] { ' ' }, 2)[0]))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.FullName.Split(new[] { ' ' }, 2)[1]));

            CreateMap<Order, OrderDto>()
                .ReverseMap();

            CreateMap<OrderDetail, OrderDetailDto>()
                .ReverseMap();
        }
    }
}


