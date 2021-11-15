using AutoMapper;
using PlatformService.Dto;
using PlatformService.Models;

namespace PlatformService.Mapping;

public class PlatformsProfile : Profile
{
    public PlatformsProfile()
    {
        CreateMap<Platform, PlatformReadDto>();
        //TODO: дождаться обновления автомаппера, где это пофиксят
        CreateMap<PlatformCreateDto, Platform>().ForCtorParam("Id", dto => dto.MapFrom(src => Guid.Empty));
        CreateMap<PlatformReadDto, PlatformPublishedDto>();
        CreateMap<Platform, GrpcPlatformModel>()
            .ForMember(dest=>dest.PlatformId, opt=>opt.MapFrom(src=>src.Id.ToString()))
            .ForMember(dest=>dest.Name, opt=>opt.MapFrom(src=>src.Name))
            .ForMember(dest=>dest.Publisher, opt=>opt.MapFrom(src=>src.Publisher));
    }
}