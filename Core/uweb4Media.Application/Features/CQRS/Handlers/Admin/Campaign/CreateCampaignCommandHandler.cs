using uweb4Media.Application.Features.CQRS.Commands.Admin.Campagin;
using Uweb4Media.Application.Features.CQRS.Commands.Admin.Campaign;
using uweb4Media.Application.Interfaces;
using Uweb4Media.Domain.Enums;

namespace uweb4Media.Application.Features.CQRS.Handlers.Admin.Camping;

public class CreateCampaignCommandHandler
{
    private IRepository<Uweb4Media.Domain.Entities.Admin.Campaign.Campaign> _repository;

    public CreateCampaignCommandHandler(IRepository<Uweb4Media.Domain.Entities.Admin.Campaign.Campaign> repository)
    {
        _repository = repository;
    }
    public async Task Handle(CreateCampaignCommand command)
    {
        CampaignStatus statusEnum;
        if (!Enum.TryParse<CampaignStatus>(command.Status, out statusEnum))
        {
            statusEnum = CampaignStatus.Planned; 
        }
        var campaign = new Uweb4Media.Domain.Entities.Admin.Campaign.Campaign
        {
            Name = command.Name,
            CompanyId = command.CompanyId,
            Budget = command.Budget,
            Status = statusEnum,
            StartDate = command.StartDate,
            EndDate = command.EndDate,
            Sectors = command.Sectors,
            Channels = command.Channels,
            Region = command.Region,
            AdFormat = command.AdFormat
        };

        await _repository.CreateAsync(campaign);
    }
}