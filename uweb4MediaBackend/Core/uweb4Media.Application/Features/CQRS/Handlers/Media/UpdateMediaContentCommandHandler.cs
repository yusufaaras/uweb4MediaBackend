using uweb4Media.Application.Features.CQRS.Commands.Media;
using uweb4Media.Application.Interfaces;
using Uweb4Media.Domain.Entities;

namespace uweb4Media.Application.Features.CQRS.Handlers.Media;

public class UpdateMediaContentCommandHandler
{
    
    private IRepository<MediaContent> _repository;

    public UpdateMediaContentCommandHandler(IRepository<MediaContent> repository)
    {
        _repository = repository;
    }
    public async Task Handle(UpdateMediaContentCommand command)
    {
        var values = await _repository.GetByIdAsync(command.Id);
        values.Url = command.Url;
        values.Title = command.Title;
        values.ContentType = command.ContentType;
        values.Thumbnail = command.Thumbnail;
        values.Sector = command.Sector;
        values.Channel = command.Channel;
        values.ContentType = command.ContentType;
        await _repository.UpdateAsync(values);
    }
}