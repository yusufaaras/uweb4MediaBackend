using uweb4Media.Application.Features.CQRS.Results.Firm;
using uweb4Media.Application.Interfaces;

namespace uweb4Media.Application.Features.CQRS.Handlers.Firm;

public class GetFirmQueryHandler
{
    private readonly IRepository<Uweb4Media.Domain.Entities.Firm> _repository;

    public GetFirmQueryHandler(IRepository<Uweb4Media.Domain.Entities.Firm> repository)
    {
        _repository = repository;
    }
    public async Task<List<GetFirmQueryResult>> Handle()
    {
        var values = await _repository.GetAllAsync();
        return values.Select(x => new GetFirmQueryResult
        {
            Id = x.Id,
            FirmName = x.FirmName,
            WebSiteUrl = x.WebSiteUrl,
            Sector = x.Sector,
            LogoUrl = x.LogoUrl,
            Country = x.Country,
            AuthorizedPerson = x.AuthorizedPerson,
            AuthorizedPersonEmail = x.AuthorizedPersonEmail,
            status = x.status,
            UserId = x.UserId
        }).ToList();
    }
}