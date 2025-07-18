using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using uweb4Media.Application.Features.CQRS.Commands.Like;
using uweb4Media.Application.Features.CQRS.Handlers.Like;
using uweb4Media.Application.Features.CQRS.Queries.Like;
using uweb4Media.Application.Interfaces; // IRepository için
using Uweb4Media.Domain.Entities; // Like ve MediaContent entity'leri için
using System.Linq; // FirstOrDefaultAsync için

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
        private readonly IRepository<Like> _likeRepository; // Mevcut beğeniyi kontrol etmek için

        public LikeController(
            GetLikeQueryHandler getLikeQueryHandler,
            GetLikeByIdQueryHandler getLikeByIdQueryHandler,
            CreateLikeCommandHandler createLikeCommandHandler,
            RemoveLikeCommandHandler removeLikeCommandHandler,
            IRepository<Like> likeRepository) // Constructor'a Like Repository'yi ekleyin
        {
            _getLikeQueryHandler = getLikeQueryHandler;
            _getLikeByIdQueryHandler = getLikeByIdQueryHandler;
            _createLikeCommandHandler = createLikeCommandHandler;
            _removeLikeCommandHandler = removeLikeCommandHandler;
            _likeRepository = likeRepository;
        }

        [HttpGet]
        public async Task<IActionResult> LikeList()
        {
            var values = await _getLikeQueryHandler.Handle();
            return Ok(values);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLike(int id)
        {
            var value = await _getLikeByIdQueryHandler.Handle(new GetLikeByIdQuery(id));
            return Ok(value);
        }

        // --- Yeni TOGGLE LİKE endpoint'i ---
        [HttpPost("toggle")] // Endpoint yolu: api/Like/toggle
        public async Task<IActionResult> ToggleLike([FromBody] CreateLikeCommand command)
        {
            try
            {
                // Kullanıcının bu medya içeriği için mevcut bir beğenisi olup olmadığını kontrol et
                var existingLike = (await _likeRepository.GetAllAsync())
                                    .FirstOrDefault(l => l.UserId == command.UserId && l.MediaContentId == command.MediaContentId);

                // Handle metotlarımızdan dönecek sonuçları tutacak değişken
                (bool IsLiked, int NewLikesCount) result;

                if (existingLike != null)
                {
                    // Beğeni zaten varsa, beğeniyi kaldır (Unlike işlemi)
                    result = await _removeLikeCommandHandler.Handle(new RemoveLikeCommand(existingLike.Id));
                }
                else
                {
                    // Beğeni yoksa, yeni bir beğeni oluştur (Like işlemi)
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
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        } 

        [HttpPost] // Bu endpoint artık ToggleLike'ı çağırıyor gibi davranacak
        public async Task<IActionResult> CreateLike(CreateLikeCommand command)
        { 
            var result = await _createLikeCommandHandler.Handle(command);
            return Ok(new { IsLiked = result.IsLiked, NewLikesCount = result.NewLikesCount });
        }


        [HttpDelete("{id}")] // Bu endpoint de artık RemoveLike'ı çağırıyor gibi davranacak
        public async Task<IActionResult> RemoveLike(int id)
        {
            // Bu endpoint'e gelen 'id', Like nesnesinin kendi ID'si olmalı, MediaContentId değil.
            // Bu nedenle, komutu uygun şekilde oluşturduğunuzdan emin olun.
            var result = await _removeLikeCommandHandler.Handle(new RemoveLikeCommand(id));
            return Ok(new { IsLiked = result.IsLiked, NewLikesCount = result.NewLikesCount });
        }
    }
}