using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using uweb4Media.Application.Features.CQRS.Commands.Plans;
using uweb4Media.Application.Features.CQRS.Handlers.Plans;
using uweb4Media.Application.Features.CQRS.Queries.Plans;

namespace Uweb4Media.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlansController : ControllerBase
    {
        
            private readonly GetPlansQueryHandler _getPlansQueryHandler;
            private readonly GetPlansByIdQueryHandler _getPlansByIdQueryHandler;
            private readonly CreatePlansCommandHandler _createPlansCommandHandler;
            private readonly UpdatePlansCommandHandler _updatePlansCommandHandler;
            private readonly RemovePlansCommandHandler _removePlansCommandHandler;
    
            public PlansController(GetPlansQueryHandler getPlansQueryHandler, GetPlansByIdQueryHandler getPlansByIdQueryHandler, CreatePlansCommandHandler createPlansCommandHandler, UpdatePlansCommandHandler updatePlansCommandHandler, RemovePlansCommandHandler removePlansCommandHandler)
            {
                _getPlansQueryHandler = getPlansQueryHandler;
                _getPlansByIdQueryHandler = getPlansByIdQueryHandler;
                _createPlansCommandHandler = createPlansCommandHandler;
                _updatePlansCommandHandler = updatePlansCommandHandler;
                _removePlansCommandHandler = removePlansCommandHandler;
            }
            [HttpGet]
            [AllowAnonymous]
            public async Task<IActionResult> PlansList()
            {
                var values = await _getPlansQueryHandler.Handle();
                return Ok(values);
            }
    
            [HttpGet("{id}")]
            [AllowAnonymous]
            public async Task<IActionResult> GetPlans(int id)
            {
                var value = await _getPlansByIdQueryHandler.Handle(new GetPlansByIdQuery(id));
                return Ok(value);
            }
    
            [HttpPost]
            public async Task<IActionResult> CreatePlans(CreatePlansCommand command)
            {
                await _createPlansCommandHandler.Handle(command);
                return Ok();
            }
    
            [HttpDelete("{id}")]
            public async Task<IActionResult> RemovePlans(int id)
            {
                await _removePlansCommandHandler.Handle(new RemovePlansCommand(id));
                return Ok();
            }
    
            [HttpPut]
            public async Task<IActionResult> UpdatePlans(UpdatePlansCommand command)
            {
                await _updatePlansCommandHandler.Handle(command);
                return Ok();
            }
    }
} 