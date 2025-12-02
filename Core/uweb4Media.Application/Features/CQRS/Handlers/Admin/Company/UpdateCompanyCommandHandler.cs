using uweb4Media.Application.Features.CQRS.Commands.Admin.Company;
using uweb4Media.Application.Interfaces;
using Uweb4Media.Domain.Enums;

namespace uweb4Media.Application.Features.CQRS.Handlers.Admin.Company;

public class UpdateCompanyCommandHandler
{
    private IRepository<Uweb4Media.Domain.Entities.Admin.CompanyManagement.Company> _repository;

    public UpdateCompanyCommandHandler(IRepository<Uweb4Media.Domain.Entities.Admin.CompanyManagement.Company> repository)
    {
        _repository = repository;
    }
    public async Task Handle(UpdateCompanyCommand command)
    {
        CompanyStatus statusEnum;
        if (!Enum.TryParse<CompanyStatus>(command.Status, out statusEnum))
            statusEnum = CompanyStatus.UnderReview; // default
        var Company = await _repository.GetByIdAsync(command.Id);
        Company.Name = command.Name;
        Company.Logo = command.Logo;
        Company.Sector = command.Sector;
        Company.Country = command.Country;
        Company.ContactPerson = command.ContactPerson;
        Company.Email = command.Email;
        Company.Website = command.Website;
        Company.Status=statusEnum;
        Company.ActiveCampaigns = command.ActiveCampaigns;
        Company.TotalSpend = command.TotalSpend;
        Company.ContentUploaded = command.ContentUploaded;
        await _repository.UpdateAsync(Company);
        
    }
}