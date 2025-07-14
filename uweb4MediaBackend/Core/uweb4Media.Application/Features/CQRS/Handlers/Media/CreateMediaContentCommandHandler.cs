using uweb4Media.Application.Features.CQRS.Commands.Media;
using uweb4Media.Application.Interfaces;
using Uweb4Media.Domain.Entities;

namespace uweb4Media.Application.Features.CQRS.Handlers.Media;

public class CreateMediaContentCommandHandler
{
    private IRepository<MediaContent> _repository;

    public CreateMediaContentCommandHandler(IRepository<MediaContent> repository)
    {
        _repository = repository;
    }
    public async Task Handle(CreateMediaContentCommand command)
    {
        await _repository.CreateAsync(new MediaContent
        { 
            UserId = command.UserId,
            Url = command.Url,
            Title = command.Title,
            Sector = command.Sector,    
            ContentType = command.ContentType,
            Channel = command.Channel,
            Thumbnail = command.Thumbnail
        });
    }
}