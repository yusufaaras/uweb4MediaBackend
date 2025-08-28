using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using uweb4Media.Application.Features.CQRS.Commands.Admin.Campagin;
using uweb4Media.Application.Features.CQRS.Commands.Admin.Video;
using uweb4Media.Application.Features.CQRS.Handlers.Admin.Video;
using uweb4Media.Application.Features.CQRS.Queries.Admin.Video;

namespace Uweb4Media.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoController : ControllerBase
    {
        private readonly GetVideoQueryHandler _getVideoQueryHandler;
        private readonly GetVideoByIdQueryHandler _getVideoByIdQueryHandler;
        private readonly CreateVideoCommandHandler _createVideoCommandHandler;
        private readonly UpdateVideoCommandHandler _updateVideoCommandHandler;
        private readonly RemoveVideoCommandHandler _removeVideoCommandHandler;
        private readonly SearchVideoQueryHandler _searchVideoQueryHandler;

        public VideoController(
            GetVideoQueryHandler getVideoQueryHandler,
            GetVideoByIdQueryHandler getVideoByIdQueryHandler,
            CreateVideoCommandHandler createVideoCommandHandler,
            UpdateVideoCommandHandler updateVideoCommandHandler,
            RemoveVideoCommandHandler removeVideoCommandHandler,
            SearchVideoQueryHandler searchVideoQueryHandler)
        {
            _getVideoQueryHandler = getVideoQueryHandler;
            _getVideoByIdQueryHandler = getVideoByIdQueryHandler;
            _createVideoCommandHandler = createVideoCommandHandler;
            _updateVideoCommandHandler = updateVideoCommandHandler;
            _removeVideoCommandHandler = removeVideoCommandHandler;
            _searchVideoQueryHandler = searchVideoQueryHandler;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> VideoList()
        {
            var values = await _getVideoQueryHandler.Handle();
            return Ok(values);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetVideo(int id)
        {
            var value = await _getVideoByIdQueryHandler.Handle(new GetVideoByIdQuery(id));
            return Ok(value);
        }

        // Arama endpoint'i eklendi
        [HttpGet("search")]
        [AllowAnonymous]
        public async Task<IActionResult> SearchVideos([FromQuery] string q)
        {
            if (string.IsNullOrWhiteSpace(q))
                return BadRequest("Arama kelimesi zorunludur.");

            var result = await _searchVideoQueryHandler.Handle(new SearchVideoQuery(q));
            return Ok(result);
        }

        // Eğer sadece admin video ekleyebilsin istiyorsan alttaki [Authorize(Roles = "Admin")]'i aktifleştir.
        // [Authorize(Roles = "Admin")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateVideo(CreateVideoCommand command)
        {
            await _createVideoCommandHandler.Handle(command);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> RemoveVideo(int id)
        {
            var video = await _getVideoByIdQueryHandler.Handle(new GetVideoByIdQuery(id));
            if (video == null)
                return NotFound();

            var userClaims = HttpContext.User;

            // 1. Admin mi?
            var isAdmin = userClaims.IsInRole("Admin") ||
                          userClaims.Claims.Any(c => c.Type == "AppRoleID" && c.Value == "1");

            // 2. Kullanıcı id'si token'dan çekiliyor (AppUserID veya sub claim)
            var userIdClaim = userClaims.Claims.FirstOrDefault(c => c.Type == "AppUserID" || c.Type == "sub")?.Value;
            int.TryParse(userIdClaim, out var userId);

            if (!isAdmin && video.UserId != userId)
                return Forbid();

            await _removeVideoCommandHandler.Handle(new RemoveVideoCommand(id));
            return Ok();
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateVideo(UpdateVideoCommand command)
        {
            await _updateVideoCommandHandler.Handle(command);
            return Ok();
        }
    }
}