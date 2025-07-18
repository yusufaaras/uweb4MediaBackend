using uweb4Media.Application.Features.CQRS.Queries.Firm;
using uweb4Media.Application.Features.CQRS.Results.Firm;
using uweb4Media.Application.Interfaces;

namespace uweb4Media.Application.Features.CQRS.Handlers.Firm;

public class GetFirmByIdQueryHandler
{
    private readonly IRepository<Uweb4Media.Domain.Entities.Firm> _repository;
    public GetFirmByIdQueryHandler(IRepository<Uweb4Media.Domain.Entities.Firm> repository)
    {
        _repository = repository;
    }
    public async Task<GetFirmByIdQueryResult> Handle(GetFirmByIdQuery query)
    {
        var values = await _repository.GetByIdAsync(query.Id);
        return new GetFirmByIdQueryResult
        {
            Id = values.Id,
            FirmName = values.FirmName,
            WebSiteUrl = values.WebSiteUrl,
            LogoUrl = values.LogoUrl,
            Sector = values.Sector,
            Country = values.Country,
            AuthorizedPerson = values.AuthorizedPerson,
            AuthorizedPersonEmail = values.AuthorizedPersonEmail,
            status = values.status,
            UserId = values.UserId
        };
    }
}