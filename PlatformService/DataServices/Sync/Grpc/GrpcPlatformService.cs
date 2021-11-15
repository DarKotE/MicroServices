using AutoMapper;
using Grpc.Core;
using PlatformService.Data;

namespace PlatformService.DataServices.Sync.Grpc;

public sealed class GrpcPlatformService : GrpcPlatform.GrpcPlatformBase
{
    private readonly IPlatformRepo _repo;
    private readonly IMapper _mapper;

    public GrpcPlatformService(IPlatformRepo repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public override Task<PlatformResponse> GetAllPlatforms(GetAllRequest getAllRequest, ServerCallContext callContext)
    {
        var response = new PlatformResponse();
        var platforms = _repo.GetAll();

        response.Platform.AddRange(_mapper.Map<IEnumerable<GrpcPlatformModel>>(platforms));

        return Task.FromResult(response);
    }
}