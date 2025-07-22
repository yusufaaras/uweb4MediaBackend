using uweb4Media.Application.Features.CQRS.Commands.Admin.Channel;
using uweb4Media.Application.Interfaces;

namespace uweb4Media.Application.Features.CQRS.Handlers.Admin.Channel;

public class UpdateChannelCommandHandler
{
    private IRepository<Uweb4Media.Domain.Entities.Admin.Channel.Channel> _repository;

    public UpdateChannelCommandHandler(IRepository<Uweb4Media.Domain.Entities.Admin.Channel.Channel> repository)
    {
        _repository = repository;
    }
    public async Task Handle(UpdateChannelCommand command)
    {
        var Channel = await _repository.GetByIdAsync(command.Id);
        Channel.Name = command.Name;
        await _repository.UpdateAsync(Channel);
        
    }
}