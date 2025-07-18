using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using uweb4Media.Application.Features.CQRS.Commands.Firm;
using uweb4Media.Application.Features.CQRS.Handlers.Firm;
using uweb4Media.Application.Features.CQRS.Queries.Firm;

namespace Uweb4Firm.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FirmController : ControllerBase
    {
        
            private readonly GetFirmQueryHandler _getFirmQueryHandler;
            private readonly GetFirmByIdQueryHandler _getFirmByIdQueryHandler;
            private readonly CreateFirmCommandHandler _createFirmCommandHandler;
            private readonly UpdateFirmCommandHandler _updateFirmCommandHandler;
            private readonly RemoveFirmCommandHandler _removeFirmCommandHandler;
    
            public FirmController(GetFirmQueryHandler getFirmQueryHandler, GetFirmByIdQueryHandler getFirmByIdQueryHandler, CreateFirmCommandHandler createFirmCommandHandler, UpdateFirmCommandHandler updateFirmCommandHandler, RemoveFirmCommandHandler removeFirmCommandHandler)
            {
                _getFirmQueryHandler = getFirmQueryHandler;
                _getFirmByIdQueryHandler = getFirmByIdQueryHandler;
                _createFirmCommandHandler = createFirmCommandHandler;
                _updateFirmCommandHandler = updateFirmCommandHandler;
                _removeFirmCommandHandler = removeFirmCommandHandler;
            }
            [HttpGet]
            [AllowAnonymous]
            public async Task<IActionResult> FirmList()
            {
                var values = await _getFirmQueryHandler.Handle();
                return Ok(values);
            }
    
            [HttpGet("{id}")]
            [AllowAnonymous]
            public async Task<IActionResult> GetFirm(int id)
            {
                var value = await _getFirmByIdQueryHandler.Handle(new GetFirmByIdQuery(id));
                return Ok(value);
            }
    
            [HttpPost]
            public async Task<IActionResult> CreateFirm(CreateFirmCommand command)
            {
                await _createFirmCommandHandler.Handle(command);
                return Ok();
            }
    
            [HttpDelete("{id}")]
            public async Task<IActionResult> RemoveFirm(int id)
            {
                await _removeFirmCommandHandler.Handle(new RemoveFirmCommand(id));
                return Ok();
            }
    
            [HttpPut]
            public async Task<IActionResult> UpdateFirm(UpdateFirmCommand command)
            {
                await _updateFirmCommandHandler.Handle(command);
                return Ok();
            }
    }
} 

