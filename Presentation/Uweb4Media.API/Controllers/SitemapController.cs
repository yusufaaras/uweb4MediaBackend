using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using uweb4Media.Application.Features.CQRS.Handlers.Admin.Video;

namespace Uweb4Media.API.Controllers
{
    [Route("sitemap.xml")]
    [ApiController]
    public class SitemapController : ControllerBase
    {
        private readonly GetVideoQueryHandler _getVideoQueryHandler;

        public SitemapController(GetVideoQueryHandler getVideoQueryHandler)
        {
            _getVideoQueryHandler = getVideoQueryHandler;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var videos = await _getVideoQueryHandler.Handle();

            var sb = new StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            sb.AppendLine("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">");
            foreach (var video in videos)
            {
                // Slug yoksa, sadece video-{id} kullan
                var url = $"https://prime.uweb4.com/content/video-{video.Id}";
                sb.AppendLine("  <url>");
                sb.AppendLine($"    <loc>{url}</loc>");
                sb.AppendLine($"    <lastmod>{video.Date:yyyy-MM-dd}</lastmod>");
                sb.AppendLine("  </url>");
            }
            sb.AppendLine("</urlset>");
            return Content(sb.ToString(), "application/xml");
        }
    }
}