using uweb4Media.Application.Features.CQRS.Commands.Notification;
using uweb4Media.Application.Interfaces;
using Uweb4Media.Domain.Entities;

namespace uweb4Media.Application.Features.CQRS.Handlers.User;

public class CreateNotificationCommandHandler
{
    private IRepository<Notification> _repository;

    public CreateNotificationCommandHandler(IRepository<Notification> repository)
    {
        _repository = repository;
    }
    public async Task Handle(CreateNotificationCommand command)
    {
        await _repository.CreateAsync(new Notification
        {
            Id = Guid.NewGuid(), 
            UserId = command.UserId,
            Message = command.Message,
            Type = command.Type,
            IsRead = command.IsRead,
            NotificationDate = command.NotificationDate ?? DateTime.UtcNow 
        });
    }
}