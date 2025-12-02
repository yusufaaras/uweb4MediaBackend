using uweb4Media.Application.Features.CQRS.Commands.Admin.Campagin;
using uweb4Media.Application.Features.CQRS.Commands.Admin.Channel;
using uweb4Media.Application.Interfaces;

namespace uweb4Media.Application.Features.CQRS.Handlers.Admin.Channel;

public class RemoveChannelCommandHandler
{
    private readonly IRepository<Uweb4Media.Domain.Entities.Admin.Channel.Channel> _repository;

    public RemoveChannelCommandHandler(IRepository<Uweb4Media.Domain.Entities.Admin.Channel.Channel> repository)
    {
        _repository = repository;
    }

    public async Task Handle(RemoveChannelCommand command)
    {
        var value = await _repository.GetByIdAsync(command.Id);
        await _repository.RemoveAsync(value);
    }
}