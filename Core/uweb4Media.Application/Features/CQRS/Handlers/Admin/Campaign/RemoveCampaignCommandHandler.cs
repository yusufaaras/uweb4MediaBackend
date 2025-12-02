using uweb4Media.Application.Features.CQRS.Commands.Admin.Campagin;
using uweb4Media.Application.Interfaces;

namespace uweb4Media.Application.Features.CQRS.Handlers.Admin.Camping;

public class RemoveCampaignCommandHandler
{
    private readonly IRepository<Uweb4Media.Domain.Entities.Admin.Campaign.Campaign> _repository;

    public RemoveCampaignCommandHandler(IRepository<Uweb4Media.Domain.Entities.Admin.Campaign.Campaign> repository)
    {
        _repository = repository;
    }

    public async Task Handle(RemoveCampaginCommand command)
    {
        var value = await _repository.GetByIdAsync(command.Id);
        await _repository.RemoveAsync(value);
    }
}