using uweb4Media.Application.Features.CQRS.Commands.Admin.Video;
using uweb4Media.Application.Features.CQRS.Commands.User;
using uweb4Media.Application.Interfaces;
using Uweb4Media.Domain.Entities.Admin.Video;

namespace uweb4Media.Application.Features.CQRS.Handlers.Admin.Video;

public class CreateVideoCommandHandler
{
    private IRepository<Uweb4Media.Domain.Entities.Admin.Video.Video> _repository;

    public CreateVideoCommandHandler(IRepository<Uweb4Media.Domain.Entities.Admin.Video.Video> repository)
    {
        _repository = repository;
    }

    public async Task Handle(CreateVideoCommand command)
    {
        var video = new Uweb4Media.Domain.Entities.Admin.Video.Video
        { 
            UserId = command.UserId,
            Link = command.Link,
            Thumbnail = command.Thumbnail,
            Sector = command.Sector,
            Channel = command.Channel,
            ContentType = command.ContentType,
            PublishStatus = "Incelemede", 
            Tags = command.Tags,
            IsPremium = command.IsPremium,
            Date = command.Date ?? DateTime.UtcNow,
            Responsible = command.Responsible,
            CompanyId = command.CompanyId,
            LikesCount = 0,
            CommentsCount = 0, 
        };

        foreach (var localizedDto in command.LocalizedData)
        {
            video.LocalizedStrings.Add(new VideoLocalizedString
            {  
                Title = localizedDto.Title,
                Description = localizedDto.Description,
            });
        }

        await _repository.CreateAsync(video); 
    }
}