using uweb4Media.Application.Features.CQRS.Queries.Admin.Company; 
using uweb4Media.Application.Features.CQRS.Results.Company;
using uweb4Media.Application.Interfaces;

namespace uweb4Media.Application.Features.CQRS.Handlers.Admin.Company;

public class GetCompanyByIdQueryHandler
{
    private readonly IRepository<Uweb4Media.Domain.Entities.Admin.CompanyManagement.Company> _repository;
    public GetCompanyByIdQueryHandler(IRepository<Uweb4Media.Domain.Entities.Admin.CompanyManagement.Company> repository)
    {
        _repository = repository;
    }
    public async Task<GetCompanyByIdQueryResult> Handle(GetCompanyByIdQuery query)
    {
        var values = await _repository.GetByIdAsync(query.Id);
        return new GetCompanyByIdQueryResult
        {
            Id = values.Id,
            Name = values.Name,
            Logo = values.Logo,
            Sector = values.Sector,
            Country = values.Country,
            ContactPerson = values.ContactPerson,
            Email = values.Email,
            Website = values.Website,
            Status = values.Status.ToString(), // Enum tipini doğrudan atayın
            RegistrationDate = values.RegistrationDate,
            ActiveCampaigns = values.ActiveCampaigns,
            TotalSpend = values.TotalSpend,
            ContentUploaded = values.ContentUploaded
        };
    }
}