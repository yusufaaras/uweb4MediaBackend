using uweb4Media.Application.Features.CQRS.Results.Admin.Campaign;
using uweb4Media.Application.Interfaces;

namespace uweb4Media.Application.Features.CQRS.Handlers.Admin.Camping;

public class GetCampaignQueryHandler
{
    private readonly IRepository<Uweb4Media.Domain.Entities.Admin.Campaign.Campaign> _repository;

    public GetCampaignQueryHandler(IRepository<Uweb4Media.Domain.Entities.Admin.Campaign.Campaign> repository)
    {
        _repository = repository;
    }
    public async Task<List<GetCampaignQueryResult>> Handle()
    {
        var values = await _repository.GetAllAsync();
        return values.Select(x => new GetCampaignQueryResult
        {
            Id = x.Id,
            Name = x.Name,
            CompanyId = x.CompanyId, 
            Budget = x.Budget,
            Status = x.Status.ToString(),
            StartDate = x.StartDate,
            EndDate = x.EndDate,
            Sectors = x.Sectors?.Select(s => s.ToString()).ToList(), 
            Channels = x.Channels?.Select(c => c.ToString()).ToList(), 
            Region = x.Region,
            AdFormat = x.AdFormat
        }).ToList();
    }
}