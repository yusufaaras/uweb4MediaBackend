using uweb4Media.Application.Features.CQRS.Commands.Admin.Campagin;
using uweb4Media.Application.Features.CQRS.Commands.Admin.Company;
using uweb4Media.Application.Interfaces;

namespace uweb4Media.Application.Features.CQRS.Handlers.Admin.Company;

public class RemoveCompanyCommandHandler
{
    private readonly IRepository<Uweb4Media.Domain.Entities.Admin.CompanyManagement.Company> _repository;

    public RemoveCompanyCommandHandler(IRepository<Uweb4Media.Domain.Entities.Admin.CompanyManagement.Company> repository)
    {
        _repository = repository;
    }

    public async Task Handle(RemoveCompanyCommand command)
    {
        var value = await _repository.GetByIdAsync(command.Id);
        await _repository.RemoveAsync(value);
    }
}