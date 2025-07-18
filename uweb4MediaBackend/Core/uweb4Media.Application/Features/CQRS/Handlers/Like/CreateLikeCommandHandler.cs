// uweb4Media.Application.Features.CQRS.Handlers.Like/CreateLikeCommandHandler.cs

using uweb4Media.Application.Features.CQRS.Commands.Like;
using uweb4Media.Application.Interfaces;
using Uweb4Media.Domain.Entities;
using System.Threading.Tasks;

public class CreateLikeCommandHandler
{
    private readonly IRepository<Like> _likeRepository; // 'Like' entity'si için repository
    private readonly IRepository<MediaContent> _mediaContentRepository; // 'MediaContent' entity'si için repository

    public CreateLikeCommandHandler(
        IRepository<Like> likeRepository,
        IRepository<MediaContent> mediaContentRepository)
    {
        _likeRepository = likeRepository;
        _mediaContentRepository = mediaContentRepository;
    }

    public async Task<(bool IsLiked, int NewLikesCount)> Handle(CreateLikeCommand command) 
    {
        var mediaContent = await _mediaContentRepository.GetByIdAsync(command.MediaContentId);

        if (mediaContent == null)
        {
            throw new Exception($"Media content with ID {command.MediaContentId} not found.");
        }

        // 2. Yeni Like kaydını oluştur
        await _likeRepository.CreateAsync(new Like
        {
            UserId = command.UserId,
            MediaContentId = command.MediaContentId,
            LikeDate = DateTime.UtcNow 
        });
        mediaContent.LikesCount++;
        await _mediaContentRepository.UpdateAsync(mediaContent);
        return (true, mediaContent.LikesCount); 
    }
}