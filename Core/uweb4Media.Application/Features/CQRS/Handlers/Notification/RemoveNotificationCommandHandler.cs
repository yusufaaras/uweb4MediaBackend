using uweb4Media.Application.Features.CQRS.Commands.Notification;
using uweb4Media.Application.Interfaces;
using Uweb4Media.Domain.Entities;

namespace uweb4Media.Application.Features.CQRS.Handlers.User;

public class RemoveNotificationCommandHandler
{
    private readonly IRepository<Notification> _repository;
    public RemoveNotificationCommandHandler(IRepository<Notification> repository)
    {
        _repository = repository;
    }
    public async Task Handle(RemoveNotificationCommand command)
    {
        var value = await _repository.GetByIdAsync(command.Id);
        await _repository.RemoveAsync(value);
    }
}