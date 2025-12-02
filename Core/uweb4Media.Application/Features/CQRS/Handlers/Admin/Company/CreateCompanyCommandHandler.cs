using uweb4Media.Application.Features.CQRS.Commands.Admin.Company;
using uweb4Media.Application.Interfaces;
using Uweb4Media.Domain.Enums;

namespace uweb4Media.Application.Features.CQRS.Handlers.Admin.Company;

public class CreateCompanyCommandHandler
{
    private readonly IRepository<Uweb4Media.Domain.Entities.Admin.CompanyManagement.Company> _repository; // 'readonly' kullanımı iyi bir pratik

    public CreateCompanyCommandHandler(IRepository<Uweb4Media.Domain.Entities.Admin.CompanyManagement.Company> repository)
    {
        _repository = repository;
    }
    public async Task Handle(CreateCompanyCommand command)
    {
        // Status stringinden enum'a dönüşüm
        CompanyStatus statusEnum;
        if (!Enum.TryParse<CompanyStatus>(command.Status, out statusEnum))
            statusEnum = CompanyStatus.UnderReview; // default

        var company = new Uweb4Media.Domain.Entities.Admin.CompanyManagement.Company
        {
            Name = command.Name,
            Logo = command.Logo,
            Sector = command.Sector,
            Country = command.Country,
            ContactPerson = command.ContactPerson,
            Email = command.Email,
            Website = command.Website,
            Status = statusEnum,
            RegistrationDate = command.RegistrationDate ?? DateTime.UtcNow,
            ActiveCampaigns = command.ActiveCampaigns,
            TotalSpend = command.TotalSpend,
            ContentUploaded = command.ContentUploaded
        };

        await _repository.CreateAsync(company);
    }
}