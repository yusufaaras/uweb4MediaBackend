using Microsoft.AspNetCore.Mvc;
using Uweb4Media.Application.Services;
using Uweb4Media.Domain.Enums;

namespace Uweb4Media.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MediaContentController : ControllerBase
    {
        private readonly MediaContentService _mediaService;

        public MediaContentController(MediaContentService mediaService)
        {
            _mediaService = mediaService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateMedia([FromBody] CreateMediaContentRequest request)
        {
            await _mediaService.CreateMediaContentAsync(
                request.Title,
                request.ThumbnailUrl,
                request.Sector,
                request.Channel,
                request.Type,
                request.YoutubeVideoId,
                request.UserId
            );

            return Ok("Media content created successfully.");
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMediaContents()
        {
            var mediaList = await _mediaService.GetAllMediaContentsAsync();
            return Ok(mediaList);
        }
    }

    public class CreateMediaContentRequest
    {
        public string Title { get; set; } = null!;
        public string ThumbnailUrl { get; set; } = null!;
        public Sector Sector { get; set; }
        public Channel Channel { get; set; }
        public ContentType Type { get; set; }
        public string? YoutubeVideoId { get; set; }
        public Guid UserId { get; set; }
    }
}