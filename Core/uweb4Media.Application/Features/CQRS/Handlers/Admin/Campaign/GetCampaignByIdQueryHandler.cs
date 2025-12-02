using uweb4Media.Application.Features.CQRS.Queries.Admin.Campaign;
using uweb4Media.Application.Features.CQRS.Results.Admin.Campaign;
using uweb4Media.Application.Interfaces;

namespace uweb4Media.Application.Features.CQRS.Handlers.Admin.Camping;

public class GetCampaignByIdQueryHandler
{
    private readonly IRepository<Uweb4Media.Domain.Entities.Admin.Campaign.Campaign> _repository;
    public GetCampaignByIdQueryHandler(IRepository<Uweb4Media.Domain.Entities.Admin.Campaign.Campaign> repository)
    {
        _repository = repository;
    }
    public async Task<GetCampaignByIdQueryResult> Handle(GetCampaignByIdQuery query)
    {
        var values = await _repository.GetByIdAsync(query.Id);
        return new GetCampaignByIdQueryResult
        {
            Id = values.Id,
            Name = values.Name,
            CompanyId = values.CompanyId,
            Budget = values.Budget, 
            Status = values.Status.ToString(),
            StartDate = values.StartDate,
            EndDate = values.EndDate,
            Sectors = values.Sectors,
            Channels = values.Channels,
            Region = values.Region,
            AdFormat = values.AdFormat,
        };
    }
}