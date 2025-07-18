using uweb4Media.Application.Features.CQRS.Commands.Comments;
using uweb4Media.Application.Interfaces; // IRepository interface'inizin yolu
using Uweb4Media.Domain.Entities;       // Comment ve MediaContent entity'lerinizin yolu
using System.Threading.Tasks;           // Task için

namespace uweb4Media.Application.Features.CQRS.Handlers.Comments;

public class CreateCommentCommandHandler
{
    private readonly IRepository<Comment> _commentRepository;        // Yorumlar için repository
    private readonly IRepository<MediaContent> _mediaContentRepository; // Medya içerikleri için repository

    // Constructor'a her iki repository'yi de enjekte edin
    public CreateCommentCommandHandler(
        IRepository<Comment> commentRepository,
        IRepository<MediaContent> mediaContentRepository)
    {
        _commentRepository = commentRepository;
        _mediaContentRepository = mediaContentRepository;
    }

    public async Task Handle(CreateCommentCommand command)
    {
        // 1. Yeni yorumu oluştur
        var comment = new Comment
        {
            UserId = command.UserId,
            Text = command.Text,
            MediaContentId = command.MediaContentId,
            CommentDate = DateTime.UtcNow // Yorum tarihini eklemeyi unutmayın
        };
        await _commentRepository.CreateAsync(comment);

        // 2. İlgili MediaContent'ı bul ve CommentsCount'ı artır
        var mediaContent = await _mediaContentRepository.GetByIdAsync(command.MediaContentId);
        if (mediaContent != null)
        {
            mediaContent.CommentsCount++;  
            await _mediaContentRepository.UpdateAsync(mediaContent); 
        } 
    }
}