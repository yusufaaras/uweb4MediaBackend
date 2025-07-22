using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc; 
using uweb4Media.Application.Features.CQRS.Commands.Admin.Sector;
using uweb4Media.Application.Features.CQRS.Handlers.Admin.Sector;
using uweb4Media.Application.Features.CQRS.Queries.Admin.Sector;

namespace Uweb4Media.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SectorController : ControllerBase
    {
        
            private readonly GetSectorQueryHandler _getSectorQueryHandler;
            private readonly GetSectorByIdQueryHandler _getSectorByIdQueryHandler;
            private readonly CreateSectorCommandHandler _createSectorCommandHandler;
            private readonly UpdateSectorCommandHandler _updateSectorCommandHandler;
            private readonly RemoveSectorCommandHandler _removeSectorCommandHandler;
    
            public SectorController(GetSectorQueryHandler getSectorQueryHandler, GetSectorByIdQueryHandler getSectorByIdQueryHandler, CreateSectorCommandHandler createSectorCommandHandler, UpdateSectorCommandHandler updateSectorCommandHandler, RemoveSectorCommandHandler removeSectorCommandHandler)
            {
                _getSectorQueryHandler = getSectorQueryHandler;
                _getSectorByIdQueryHandler = getSectorByIdQueryHandler;
                _createSectorCommandHandler = createSectorCommandHandler;
                _updateSectorCommandHandler = updateSectorCommandHandler;
                _removeSectorCommandHandler = removeSectorCommandHandler;
            }
            [HttpGet]
            [AllowAnonymous]
            public async Task<IActionResult> SectorList()
            {
                var values = await _getSectorQueryHandler.Handle();
                return Ok(values);
            }
    
            [HttpGet("{id}")]
            [AllowAnonymous]
            public async Task<IActionResult> GetSector(int id)
            {
                var value = await _getSectorByIdQueryHandler.Handle(new GetSectorByIdQuery(id));
                return Ok(value);
            }
    
            [HttpPost]
            public async Task<IActionResult> CreateSector(CreateSectorCommand command)
            {
                await _createSectorCommandHandler.Handle(command);
                return Ok();
            }
    
            [HttpDelete("{id}")]
            public async Task<IActionResult> RemoveSector(int id)
            {
                await _removeSectorCommandHandler.Handle(new RemoveSectorCommand(id));
                return Ok();
            }
    
            [HttpPut]
            public async Task<IActionResult> UpdateSector(UpdateSectorCommand command)
            {
                await _updateSectorCommandHandler.Handle(command);
                return Ok();
            }
    }
}