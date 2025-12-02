using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using uweb4Media.Application.Features.CQRS.Commands.Admin.Campagin;
using uweb4Media.Application.Features.CQRS.Commands.Admin.Video;
using uweb4Media.Application.Features.CQRS.Handlers.Admin.Video;
using uweb4Media.Application.Features.CQRS.Queries.Admin.Video;
using uweb4Media.Application.Services.Indexing; // <-- Servis importu

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
        private readonly GetVideoNLWebJsonQueryHandler _getVideoNLWebJsonQueryHandler;
        private readonly SearchEnginePingService _pingService; // <-- Servis field'ı

        private const string SitemapUrl = "https://prime.uweb4.com/sitemap.xml"; // Sitemap URL sabiti

        public VideoController(
            GetVideoQueryHandler getVideoQueryHandler,
            GetVideoByIdQueryHandler getVideoByIdQueryHandler,
            CreateVideoCommandHandler createVideoCommandHandler,
            UpdateVideoCommandHandler updateVideoCommandHandler,
            RemoveVideoCommandHandler removeVideoCommandHandler,
            SearchVideoQueryHandler searchVideoQueryHandler,
            GetVideoNLWebJsonQueryHandler getVideoNlWebJsonQueryHandler,
            SearchEnginePingService pingService // <-- Servis injection
        )
        {
            _getVideoQueryHandler = getVideoQueryHandler;
            _getVideoByIdQueryHandler = getVideoByIdQueryHandler;
            _createVideoCommandHandler = createVideoCommandHandler;
            _updateVideoCommandHandler = updateVideoCommandHandler;
            _removeVideoCommandHandler = removeVideoCommandHandler;
            _searchVideoQueryHandler = searchVideoQueryHandler;
            _getVideoNLWebJsonQueryHandler = getVideoNlWebJsonQueryHandler;
            _pingService = pingService; // <-- Atama
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> VideoList()
        {
            var values = await _getVideoQueryHandler.Handle();
            // Eğer tüm GET'lerde de ping atmak istiyorsan, aşağıdaki satırı aç:
            // await _pingService.PingAllAsync(SitemapUrl);
            return Ok(values);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetVideo(int id)
        {
            var value = await _getVideoByIdQueryHandler.Handle(new GetVideoByIdQuery(id));
            // await _pingService.PingAllAsync(SitemapUrl);
            return Ok(value);
        }

        [HttpGet("search")]
        [AllowAnonymous]
        public async Task<IActionResult> SearchVideos([FromQuery] string q)
        {
            if (string.IsNullOrWhiteSpace(q))
                return BadRequest("Arama kelimesi zorunludur.");

            var result = await _searchVideoQueryHandler.Handle(new SearchVideoQuery(q));
            // await _pingService.PingAllAsync(SitemapUrl);
            return Ok(result);
        }

        [HttpGet("{id}/nlweb.json")]
        [AllowAnonymous]
        public async Task<IActionResult> GetVideoNLWebJson(int id)
        {
            var nlwebJson = await _getVideoNLWebJsonQueryHandler.Handle(id);
            if (nlwebJson == null)
                return NotFound(new { error = "Not found" });
            // await _pingService.PingAllAsync(SitemapUrl);
            return new JsonResult(nlwebJson);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateVideo(CreateVideoCommand command)
        {
            await _createVideoCommandHandler.Handle(command);
            await _pingService.PingAllAsync(SitemapUrl); // Ping işlemi
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

            var isAdmin = userClaims.IsInRole("Admin") ||
                          userClaims.Claims.Any(c => c.Type == "AppRoleID" && c.Value == "1");

            var userIdClaim = userClaims.Claims.FirstOrDefault(c => c.Type == "AppUserID" || c.Type == "sub")?.Value;
            int.TryParse(userIdClaim, out var userId);

            if (!isAdmin && video.UserId != userId)
                return Forbid();

            await _removeVideoCommandHandler.Handle(new RemoveVideoCommand(id));
            await _pingService.PingAllAsync(SitemapUrl); // Ping işlemi
            return Ok();
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateVideo(UpdateVideoCommand command)
        {
            await _updateVideoCommandHandler.Handle(command);
            await _pingService.PingAllAsync(SitemapUrl); // Ping işlemi
            return Ok();
        }
    }
}