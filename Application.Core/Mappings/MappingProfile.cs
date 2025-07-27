using AutoMapper;
using Domain;

namespace Application.Mappings
{
    public sealed class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Address mappings
            CreateMap<Address, AddressDto>().ReverseMap();
        }
    }
}