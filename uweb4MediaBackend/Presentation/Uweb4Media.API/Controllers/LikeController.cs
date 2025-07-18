using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using uweb4Media.Application.Features.CQRS.Commands.Like;
using uweb4Media.Application.Features.CQRS.Handlers.Like;
using uweb4Media.Application.Features.CQRS.Queries.Like;
using uweb4Media.Application.Interfaces;
using Uweb4Media.Domain.Entities;
using System.Linq;
using System.Threading.Tasks; // Task kullanımı için
using System; // Exception için

namespace Uweb4Media.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikeController : ControllerBase
    {
        private readonly GetLikeQueryHandler _getLikeQueryHandler;
        private readonly GetLikeByIdQueryHandler _getLikeByIdQueryHandler;
        private readonly CreateLikeCommandHandler _createLikeCommandHandler;
        private readonly RemoveLikeCommandHandler _removeLikeCommandHandler;
        private readonly IRepository<Like> _likeRepository;

        public LikeController(
            GetLikeQueryHandler getLikeQueryHandler,
            GetLikeByIdQueryHandler getLikeByIdQueryHandler,
            CreateLikeCommandHandler createLikeCommandHandler,
            RemoveLikeCommandHandler removeLikeCommandHandler,
            IRepository<Like> likeRepository)
        {
            _getLikeQueryHandler = getLikeQueryHandler;
            _getLikeByIdQueryHandler = getLikeByIdQueryHandler;
            _createLikeCommandHandler = createLikeCommandHandler;
            _removeLikeCommandHandler = removeLikeCommandHandler;
            _likeRepository = likeRepository;
        }

        // LikeList metodunu güncelliyoruz: İsteğe bağlı userId parametresi ekliyoruz
        [HttpGet]
        public async Task<IActionResult> LikeList([FromQuery] int? userId)
        {
            if (userId.HasValue)
            {
                // Eğer userId parametresi geldiyse, o kullanıcıya ait beğenileri filtrele
                var userLikes = (await _likeRepository.GetAllAsync())
                                    .Where(l => l.UserId == userId.Value)
                                    .ToList(); // ToList ekledim, GetAllAsync bir IEnumerable döndürebilir.
                return Ok(userLikes);
            }
            else
            {
                // userId parametresi gelmediyse, tüm beğenileri döndür (mevcut davranış)
                // Dikkat: Genellikle bu herkesin beğenisini döndürmek yerine,
                // ya yetkilendirme ile sadece adminlerin erişebileceği bir endpoint olur,
                // ya da bu durum engellenir. Şimdilik sizin isteğiniz doğrultusunda tutuyorum.
                var values = await _getLikeQueryHandler.Handle();
                return Ok(values);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLike(int id)
        {
            var value = await _getLikeByIdQueryHandler.Handle(new GetLikeByIdQuery(id));
            return Ok(value);
        }

        [HttpPost("toggle")]
        public async Task<IActionResult> ToggleLike([FromBody] CreateLikeCommand command)
        {
            try
            {
                var existingLike = (await _likeRepository.GetAllAsync())
                                    .FirstOrDefault(l => l.UserId == command.UserId && l.MediaContentId == command.MediaContentId);

                (bool IsLiked, int NewLikesCount) result;

                if (existingLike != null)
                {
                    result = await _removeLikeCommandHandler.Handle(new RemoveLikeCommand(existingLike.Id));
                }
                else
                {
                    result = await _createLikeCommandHandler.Handle(new CreateLikeCommand
                    {
                        UserId = command.UserId,
                        MediaContentId = command.MediaContentId
                    });
                }

                return Ok(new { IsLiked = result.IsLiked, NewLikesCount = result.NewLikesCount });
            }
            catch (Exception ex)
            {
                // Daha detaylı loglama ve hata yönetimi eklenebilir
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost] // Bu endpoint'i ToggleLike'ı çağıracak şekilde kullanmanızı önermem, ayrı işlevleri ayrı tutun.
                   // Eğer bir Like oluşturma işlemi hala gerekiyorsa, bunu koruyun.
        public async Task<IActionResult> CreateLike(CreateLikeCommand command)
        {
            var result = await _createLikeCommandHandler.Handle(command);
            return Ok(new { IsLiked = result.IsLiked, NewLikesCount = result.NewLikesCount });
        }


        [HttpDelete("{id}")] // Bu endpoint de RemoveLike'ı çağırıyor gibi davranacak
        public async Task<IActionResult> RemoveLike(int id)
        {
            // Bu endpoint'e gelen 'id', Like nesnesinin kendi ID'si olmalı, MediaContentId değil.
            // Bu nedenle, komutu uygun şekilde oluşturduğunuzdan emin olun.
            var result = await _removeLikeCommandHandler.Handle(new RemoveLikeCommand(id));
            return Ok(new { IsLiked = result.IsLiked, NewLikesCount = result.NewLikesCount });
        }
    }
}