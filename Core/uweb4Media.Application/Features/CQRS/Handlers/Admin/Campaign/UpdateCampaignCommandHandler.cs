using Uweb4Media.Application.Features.CQRS.Commands.Admin.Campaign;
using uweb4Media.Application.Interfaces;
using Uweb4Media.Domain.Enums;

namespace uweb4Media.Application.Features.CQRS.Handlers.Admin.Camping;

public class UpdateCampaignCommandHandler
{
    private IRepository<Uweb4Media.Domain.Entities.Admin.Campaign.Campaign> _repository;

    public UpdateCampaignCommandHandler(IRepository<Uweb4Media.Domain.Entities.Admin.Campaign.Campaign> repository)
    {
        _repository = repository;
    }
    public async Task Handle(UpdateCampaignCommand command)
    {
        var campaign = await _repository.GetByIdAsync(command.Id);
        if (campaign == null)
            throw new Exception("Campaign not found");
        
        CampaignStatus statusEnum;
        if (!Enum.TryParse<CampaignStatus>(command.Status, out statusEnum))
            statusEnum = CampaignStatus.Planned;
        
        campaign.Name = command.Name;
        campaign.CompanyId = command.CompanyId;
        campaign.Budget = command.Budget;
        campaign.Status = statusEnum;
        campaign.StartDate = command.StartDate;
        campaign.EndDate = command.EndDate;
        campaign.Sectors = command.Sectors;
        campaign.Channels = command.Channels;
        campaign.Region = command.Region;
        campaign.AdFormat = command.AdFormat;

        await _repository.UpdateAsync(campaign);
        
    }
}