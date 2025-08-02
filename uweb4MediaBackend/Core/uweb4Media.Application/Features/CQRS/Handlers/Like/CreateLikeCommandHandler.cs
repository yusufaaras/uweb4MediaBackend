// uweb4Media.Application.Features.CQRS.Handlers.Like/CreateLikeCommandHandler.cs

using uweb4Media.Application.Features.CQRS.Commands.Like;
using uweb4Media.Application.Interfaces;
using Uweb4Media.Domain.Entities;
using System.Threading.Tasks;
using Uweb4Media.Domain.Entities.Admin.Video;

public class CreateLikeCommandHandler
{
    private readonly IRepository<Like> _likeRepository; // 'Like' entity'si için repository
    private readonly IRepository<Video> _VideoRepository; // 'Video' entity'si için repository

    public CreateLikeCommandHandler(
        IRepository<Like> likeRepository,
        IRepository<Video> VideoRepository)
    {
        _likeRepository = likeRepository;
        _VideoRepository = VideoRepository;
    }

    public async Task<(bool IsLiked, int NewLikesCount)> Handle(CreateLikeCommand command) 
    {
        var Video = await _VideoRepository.GetByIdAsync(command.VideoId);

        if (Video == null)
        {
            throw new Exception($"Media content with ID {command.VideoId} not found.");
        }

        // 2. Yeni Like kaydını oluştur
        await _likeRepository.CreateAsync(new Like
        {
            UserId = command.UserId,
            VideoId = command.VideoId,
            LikeDate = DateTime.UtcNow 
        });
        Video.LikesCount++;
        await _VideoRepository.UpdateAsync(Video);
        return (true, Video.LikesCount); 
    }
}