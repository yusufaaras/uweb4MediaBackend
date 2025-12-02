using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using uweb4Media.Application.Features.CQRS.Commands.Admin.Campagin;
using Uweb4Media.Application.Features.CQRS.Commands.Admin.Campaign;
using uweb4Media.Application.Features.CQRS.Handlers.Admin.Camping;
using uweb4Media.Application.Features.CQRS.Queries.Admin.Campaign;

namespace Uweb4Campaign.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampaignController : ControllerBase
    {
        
            private readonly GetCampaignQueryHandler _getCampaignQueryHandler;
            private readonly GetCampaignByIdQueryHandler _getCampaignByIdQueryHandler;
            private readonly CreateCampaignCommandHandler _createCampaignCommandHandler;
            private readonly UpdateCampaignCommandHandler _updateCampaignCommandHandler;
            private readonly RemoveCampaignCommandHandler _removeCampaignCommandHandler;
    
            public CampaignController(GetCampaignQueryHandler getCampaignQueryHandler, GetCampaignByIdQueryHandler getCampaignByIdQueryHandler, CreateCampaignCommandHandler createCampaignCommandHandler, UpdateCampaignCommandHandler updateCampaignCommandHandler, RemoveCampaignCommandHandler removeCampaignCommandHandler)
            {
                _getCampaignQueryHandler = getCampaignQueryHandler;
                _getCampaignByIdQueryHandler = getCampaignByIdQueryHandler;
                _createCampaignCommandHandler = createCampaignCommandHandler;
                _updateCampaignCommandHandler = updateCampaignCommandHandler;
                _removeCampaignCommandHandler = removeCampaignCommandHandler;
            }
            [HttpGet]
            [AllowAnonymous]
            public async Task<IActionResult> CampaignList()
            {
                var values = await _getCampaignQueryHandler.Handle();
                return Ok(values);
            }
    
            [HttpGet("{id}")]
            [AllowAnonymous]
            public async Task<IActionResult> GetCampaign(int id)
            {
                var value = await _getCampaignByIdQueryHandler.Handle(new GetCampaignByIdQuery(id));
                return Ok(value);
            }
    
            [HttpPost]
            public async Task<IActionResult> CreateCampaign(CreateCampaignCommand command)
            {
                await _createCampaignCommandHandler.Handle(command);
                return Ok();
            }
    
            [HttpDelete("{id}")]
            public async Task<IActionResult> RemoveCampaign(int id)
            {
                await _removeCampaignCommandHandler.Handle(new RemoveCampaginCommand(id));
                return Ok();
            }
    
            [HttpPut]
            public async Task<IActionResult> UpdateCampaign(UpdateCampaignCommand command)
            {
                await _updateCampaignCommandHandler.Handle(command);
                return Ok();
            }
    }
}