using AutoMapper;
using Million.Application.DTOs;
using Million.Domain.Entities;

namespace Million.Application.MapperProfile
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<CreatePropertyRequest, Property>();
            CreateMap<UpdatePropertyRequest, Property>();
            CreateMap<PropertyImage, PropertyImageDto>();
            CreateMap<Property, PropertyDto>()
                .ForMember(d => d.Images, o => o.MapFrom(s => s.Images));
        }
    }
}
