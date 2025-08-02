using uweb4Media.Application.Features.CQRS.Commands.Comments;
using uweb4Media.Application.Interfaces; // IRepository interface'inizin yolu
using Uweb4Media.Domain.Entities;       // Comment ve Video entity'lerinizin yolu
using System.Threading.Tasks;
using Uweb4Media.Domain.Entities.Admin.Video; // Task için

namespace uweb4Media.Application.Features.CQRS.Handlers.Comments;

public class CreateCommentCommandHandler
{
    private readonly IRepository<Comment> _commentRepository;        // Yorumlar için repository
    private readonly IRepository<Video> _VideoRepository; // Medya içerikleri için repository

    // Constructor'a her iki repository'yi de enjekte edin
    public CreateCommentCommandHandler(
        IRepository<Comment> commentRepository,
        IRepository<Video> VideoRepository)
    {
        _commentRepository = commentRepository;
        _VideoRepository = VideoRepository;
    }

    public async Task Handle(CreateCommentCommand command)
    {
        // 1. Yeni yorumu oluştur
        var comment = new Comment
        {
            UserId = command.UserId,
            Text = command.Text,
            VideoId = command.VideoId,
            CommentDate = DateTime.UtcNow // Yorum tarihini eklemeyi unutmayın
        };
        await _commentRepository.CreateAsync(comment);

        // 2. İlgili Video'ı bul ve CommentsCount'ı artır
        var Video = await _VideoRepository.GetByIdAsync(command.VideoId);
        if (Video != null)
        {
            Video.CommentsCount++;  
            await _VideoRepository.UpdateAsync(Video); 
        } 
    }
}