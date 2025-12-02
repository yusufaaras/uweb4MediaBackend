using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using uweb4Media.Application.Features.CQRS.Commands.Admin.Company;
using uweb4Media.Application.Features.CQRS.Handlers.Admin.Company;
using uweb4Media.Application.Features.CQRS.Queries.Admin.Company;

namespace Uweb4Media.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly GetCompanyQueryHandler _getCompanyQueryHandler;
        private readonly GetCompanyByIdQueryHandler _getCompanyByIdQueryHandler;
        private readonly CreateCompanyCommandHandler _createCompanyCommandHandler;
        private readonly UpdateCompanyCommandHandler _updateCompanyCommandHandler;
        private readonly RemoveCompanyCommandHandler _removeCompanyCommandHandler;

        public CompanyController(
            GetCompanyQueryHandler getCompanyQueryHandler,
            GetCompanyByIdQueryHandler getCompanyByIdQueryHandler,
            CreateCompanyCommandHandler createCompanyCommandHandler,
            UpdateCompanyCommandHandler updateCompanyCommandHandler,
            RemoveCompanyCommandHandler removeCompanyCommandHandler)
        {
            _getCompanyQueryHandler = getCompanyQueryHandler;
            _getCompanyByIdQueryHandler = getCompanyByIdQueryHandler;
            _createCompanyCommandHandler = createCompanyCommandHandler;
            _updateCompanyCommandHandler = updateCompanyCommandHandler;
            _removeCompanyCommandHandler = removeCompanyCommandHandler;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> CompanyList()
        {
            var values = await _getCompanyQueryHandler.Handle();
            return Ok(values);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCompany(int id)
        {
            var value = await _getCompanyByIdQueryHandler.Handle(new GetCompanyByIdQuery(id));
            return Ok(value);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateCompany(CreateCompanyCommand command)
        {
            await _createCompanyCommandHandler.Handle(command);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveCompany(int id)
        {
            await _removeCompanyCommandHandler.Handle(new RemoveCompanyCommand(id));
            return Ok();
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCompany(UpdateCompanyCommand command)
        {
            await _updateCompanyCommandHandler.Handle(command);
            return Ok();
        }
    }
}