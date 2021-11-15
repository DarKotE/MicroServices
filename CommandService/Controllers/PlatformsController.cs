using AutoMapper;
using CommandService.Data;
using CommandService.Dto;
using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers;

[Route("api/c/[controller]")]
[ApiController]
public class PlatformsController : ControllerBase
{
    private readonly ICommandRepo _repo;
    private readonly IMapper _mapper;

    public PlatformsController(ICommandRepo repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
    {
        var platformEntries = _repo.GetAllPlatforms();
        return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformEntries));
    }

    [HttpPost]
    public ActionResult TestConn()
    {
        Console.WriteLine($"{nameof(TestConn)} received POST request!");
        return Ok("Received post from platformservice");
    }
}