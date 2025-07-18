using uweb4Media.Application.Features.CQRS.Commands.Media;
using uweb4Media.Application.Interfaces; // IRepository interface'inizin yolu
using Uweb4Media.Domain.Entities; // Entity'lerinizin yolu
using System.Threading.Tasks; // Task için

namespace uweb4Media.Application.Features.CQRS.Handlers.Media;

public class CreateMediaContentCommandHandler
{
    private readonly IRepository<MediaContent> _repository; // 'readonly' kullanımı iyi bir pratik

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
            Thumbnail = command.Thumbnail,
            IsPremium = command.IsPremium,
            MetaTitle = command.MetaTitle,
            MetaDescription = command.MetaDescription,
            Duration = command.Duration,
            Excerpt = command.Excerpt,
            YoutubeVideoId = command.YoutubeVideoId,
            Tags = command.Tags,
            CreatedDate = DateTime.UtcNow,
            LikesCount = 0,
            CommentsCount = 0,
            ViewCount = 0 
        });
    }
}