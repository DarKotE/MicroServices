using AutoMapper;
using CommandService.Dto;
using CommandService.Models;
using PlatformService;

namespace CommandService.Mapping;

public class CommandsProfile : Profile
{
    public CommandsProfile()
    {
        CreateMap<CommandCreateDto, Command>();
        CreateMap<Command, CommandReadDto>();
        CreateMap<Platform, PlatformReadDto>();
        CreateMap<PlatformPublishedDto, Platform>()
            .ForMember(dest => dest.OriginId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => 0));
        CreateMap<GrpcPlatformModel, Platform>()
            .ForMember(dest => dest.OriginId, opt => opt.MapFrom(src => Guid.Parse(src.PlatformId)))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Commands, opt => opt.Ignore());
    }
}