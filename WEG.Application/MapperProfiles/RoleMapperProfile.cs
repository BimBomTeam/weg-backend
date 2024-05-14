using AutoMapper;
using WEG.Domain.Entities;
using WEG.Infrastructure.Dto;
using WEG.Infrastructure.Dto.Roles;

namespace WEG.Application.MapperProfiles
{
    public class RoleMapperProfile : Profile
    {
        public RoleMapperProfile()
        {
            CreateMap<RolesRedisDto, RoleDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));

            CreateMap<NpcRole, RoleDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
        }
    }
}
