// uweb4Media.Application.Features.CQRS.Handlers.Like/RemoveLikeCommandHandler.cs

using uweb4Media.Application.Features.CQRS.Commands.Like;
using uweb4Media.Application.Interfaces;
using Uweb4Media.Domain.Entities;
using System.Threading.Tasks;
using Uweb4Media.Domain.Entities.Admin.Video;

namespace uweb4Media.Application.Features.CQRS.Handlers.Like;

public class RemoveLikeCommandHandler
{
    private readonly IRepository<Uweb4Media.Domain.Entities.Like> _likeRepository;
    private readonly IRepository<Video> _VideoRepository; // 'Video' için repository

    public RemoveLikeCommandHandler(
        IRepository<Uweb4Media.Domain.Entities.Like> likeRepository,
        IRepository<Video> VideoRepository)
    {
        _likeRepository = likeRepository;
        _VideoRepository = VideoRepository;
    }

    public async Task<(bool IsLiked, int NewLikesCount)> Handle(RemoveLikeCommand command) 
    { 
        var likeToRemove = await _likeRepository.GetByIdAsync(command.Id);

        if (likeToRemove == null)
        { 
            throw new Exception($"Like with ID {command.Id} not found.");
        }

        // 2. İlgili Video'ı çek
        var Video = await _VideoRepository.GetByIdAsync(likeToRemove.VideoId);

        if (Video == null)
        {
            throw new Exception($"Media content with ID {likeToRemove.VideoId} not found.");
        }
 
        await _likeRepository.RemoveAsync(likeToRemove);
 
        if (Video.LikesCount > 0)
        {
            Video.LikesCount--;
        }
 
        await _VideoRepository.UpdateAsync(Video);
 
        return (false, Video.LikesCount); 
    }
}