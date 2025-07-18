// uweb4Media.Application.Features.CQRS.Handlers.Like/RemoveLikeCommandHandler.cs

using uweb4Media.Application.Features.CQRS.Commands.Like;
using uweb4Media.Application.Interfaces;
using Uweb4Media.Domain.Entities;
using System.Threading.Tasks;

namespace uweb4Media.Application.Features.CQRS.Handlers.Like;

public class RemoveLikeCommandHandler
{
    private readonly IRepository<Uweb4Media.Domain.Entities.Like> _likeRepository;
    private readonly IRepository<MediaContent> _mediaContentRepository; // 'MediaContent' için repository

    public RemoveLikeCommandHandler(
        IRepository<Uweb4Media.Domain.Entities.Like> likeRepository,
        IRepository<MediaContent> mediaContentRepository)
    {
        _likeRepository = likeRepository;
        _mediaContentRepository = mediaContentRepository;
    }

    public async Task<(bool IsLiked, int NewLikesCount)> Handle(RemoveLikeCommand command) // Geriye beğeni durumu ve sayıyı döndürelim
    {
        // 1. Silinecek Like kaydını ID'si ile çek
        var likeToRemove = await _likeRepository.GetByIdAsync(command.Id);

        if (likeToRemove == null)
        {
            // Eğer silinecek beğeni bulunamazsa, zaten beğenilmemiş demektir.
            // Bu durumda bir hata fırlatabilir veya mevcut beğenme sayısını döndürebilirsiniz.
            // Şimdilik null döndürüp hata kontrolü yapalım.
            throw new Exception($"Like with ID {command.Id} not found.");
        }

        // 2. İlgili MediaContent'ı çek
        var mediaContent = await _mediaContentRepository.GetByIdAsync(likeToRemove.MediaContentId);

        if (mediaContent == null)
        {
            throw new Exception($"Media content with ID {likeToRemove.MediaContentId} not found.");
        }

        // 3. Like kaydını sil
        await _likeRepository.RemoveAsync(likeToRemove);

        // 4. MediaContent'ın LikesCount'unu azalt (negatif olmamasına dikkat edin)
        if (mediaContent.LikesCount > 0)
        {
            mediaContent.LikesCount--;
        }

        // 5. MediaContent'taki değişikliği veritabanına kaydet
        // IRepository'nizin UpdateAsync metodunun SaveChangesAsync() çağırdığından emin olun!
        await _mediaContentRepository.UpdateAsync(mediaContent);

        // 6. İşlem sonrası durumu ve güncel beğeni sayısını döndür
        return (false, mediaContent.LikesCount); // Beğenmeme işlemi olduğu için IsLiked her zaman false
    }
}