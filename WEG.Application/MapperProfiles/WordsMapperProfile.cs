using AutoMapper;
using WEG.Domain.Entities;
using WEG.Infrastructure.Dto;

namespace WEG.Application.MapperProfiles
{
    public class WordsMapperProfile : Profile
    {
        public WordsMapperProfile()
        {
            CreateMap<WordDto, Word>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.State,
                        opt => opt.MapFrom(src => Enum.Parse(typeof(WordProgressState), src.State)))
                    .ForMember(dest => dest.RoleId,
                        opt => opt.MapFrom(src => src.RoleId));

            CreateMap<Word, WordDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.State,
                    opt => opt.MapFrom(src => src.State.ToString()))
                .ForMember(dest => dest.RoleId,
                    opt => opt.MapFrom(src => src.RoleId));
        }
    }
}
