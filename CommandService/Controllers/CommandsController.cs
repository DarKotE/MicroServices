using System.ComponentModel.Design;
using AutoMapper;
using CommandService.Data;
using CommandService.Dto;
using CommandService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers;

[Route("/api/c/platforms/{platformId}/[controller]")]
[ApiController]
public class CommandsController : ControllerBase
{
    private readonly ICommandRepo _repo;
    private readonly IMapper _mapper;

    public CommandsController(ICommandRepo repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult<IEnumerable<CommandReadDto>> GetCommandsForPlatform(int platformId)
    {
        if (!_repo.Exists(platformId))
            return NotFound();

        var commands = _repo.GetCommandsForPlatform(platformId);
        return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commands));
    }

    [HttpGet("{commandId}", Name = "GetCommandForPlatform")]
    public ActionResult<CommandReadDto> GetCommandForPlatform(int platformId, int commandId)
    {
        if (!_repo.Exists(platformId))
            return NotFound();

        var command = _repo.GetCommand(platformId, commandId);
        if (command is null)
            return NotFound();
        
        return Ok(_mapper.Map<CommandReadDto>(command));
    }
    
    [HttpPost]
    public ActionResult<CommandReadDto> CreateCommandForPlatform(int platformId, CommandCreateDto dto)
    {
        if (!_repo.Exists(platformId))
            return NotFound();

        var command = _mapper.Map<Command>(dto);
        _repo.CreateCommand(platformId, command);
        
        var saved = _repo.SaveChanges();

        var returnDto = _mapper.Map<CommandReadDto>(command);

        return saved
            ? CreatedAtRoute(nameof(GetCommandForPlatform), new {platformId, commandId = returnDto.Id}, returnDto)
            : StatusCode(StatusCodes.Status500InternalServerError);
    }
}