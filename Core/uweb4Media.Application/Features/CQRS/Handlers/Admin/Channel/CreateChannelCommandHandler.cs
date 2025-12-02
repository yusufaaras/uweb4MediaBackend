using uweb4Media.Application.Features.CQRS.Commands.Admin.Channel;
using uweb4Media.Application.Interfaces;

namespace uweb4Media.Application.Features.CQRS.Handlers.Admin.Channel;

public class CreateChannelCommandHandler
{
    private readonly IRepository<Uweb4Media.Domain.Entities.Admin.Channel.Channel> _repository; // 'readonly' kullanımı iyi bir pratik

    public CreateChannelCommandHandler(IRepository<Uweb4Media.Domain.Entities.Admin.Channel.Channel> repository)
    {
        _repository = repository;
    }
    public async Task Handle(CreateChannelCommand command)
    {
        await _repository.CreateAsync(new Uweb4Media.Domain.Entities.Admin.Channel.Channel
        {
            Name = command.Name
        });
    }
}