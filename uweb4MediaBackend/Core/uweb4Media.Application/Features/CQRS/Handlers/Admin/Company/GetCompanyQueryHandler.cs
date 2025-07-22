using uweb4Media.Application.Features.CQRS.Results.Company;
using uweb4Media.Application.Interfaces;

namespace uweb4Media.Application.Features.CQRS.Handlers.Admin.Company;

public class GetCompanyQueryHandler
{
    private readonly IRepository<Uweb4Media.Domain.Entities.Admin.CompanyManagement.Company> _repository;

    public GetCompanyQueryHandler(IRepository<Uweb4Media.Domain.Entities.Admin.CompanyManagement.Company> repository)
    {
        _repository = repository;
    }
    public async Task<List<GetCompanyQueryResult>> Handle()
    {
        var values = await _repository.GetAllAsync();
        return values.Select(x => new GetCompanyQueryResult
        {
            Id = x.Id,
            Name = x.Name,
            Logo = x.Logo,
            Sector = x.Sector,
            Country = x.Country,
            ContactPerson = x.ContactPerson,
            Email = x.Email,
            Website = x.Website,
            Status = x.Status.ToString(), // Direct mapping of enum
            RegistrationDate = x.RegistrationDate,
            ActiveCampaigns = x.ActiveCampaigns,
            TotalSpend = x.TotalSpend,
            ContentUploaded = x.ContentUploaded
        }).ToList();

    }
}