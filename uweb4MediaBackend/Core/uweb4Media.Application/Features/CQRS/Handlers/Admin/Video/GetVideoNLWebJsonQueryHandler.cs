using System.Collections.Generic;
using System.Threading.Tasks;
using uweb4Media.Application.Interfaces;
using Uweb4Media.Domain.Entities.Admin.Video;

namespace uweb4Media.Application.Features.CQRS.Handlers.Admin.Video
{
    public class GetVideoNLWebJsonQueryHandler
    {
        private readonly IRepository<Uweb4Media.Domain.Entities.Admin.Video.Video> _repository;

        public GetVideoNLWebJsonQueryHandler(IRepository<Uweb4Media.Domain.Entities.Admin.Video.Video> repository)
        {
            _repository = repository;
        }

        public async Task<object?> Handle(int id)
        {
            // Video'yu veritabanından çek
            var video = await _repository.GetByIdAsync(id);
            if (video == null) return null;

            string publisherName = !string.IsNullOrWhiteSpace(video.Responsible)
                ? video.Responsible!
                : (video.CompanyId != null ? video.Company?.Name ?? "Unknown" : "Unknown");

            string authorName = !string.IsNullOrWhiteSpace(video.Responsible)
                ? video.Responsible!
                : (video.User?.Username ?? "Unknown");

            var tags = video.Tags ?? new List<string>();

            // Youtube embed ID çıkarımı
            string embedUrl = null;
            if (!string.IsNullOrWhiteSpace(video.Link))
            {
                if (video.Link.Contains("youtube.com/watch?v="))
                {
                    var videoId = video.Link.Split("v=").Length > 1
                        ? video.Link.Split("v=")[1].Split('&')[0]
                        : "";
                    embedUrl = $"https://www.youtube.com/embed/{videoId}";
                }
                else
                {
                    embedUrl = video.Link;
                }
            }

            var nlwebJson = new Dictionary<string, object?>
            {
                ["@context"] = "https://schema.org",
                ["@type"] = "VideoObject",
                ["headline"] = video.Title,
                ["name"] = video.Title,
                ["publisher"] = new Dictionary<string, object?>
                {
                    ["@type"] = "Organization",
                    ["name"] = publisherName
                },
                ["datePublished"] = video.Date,         
                ["uploadDate"] = video.Date,           
                ["description"] = video.Description,
                ["keywords"] = tags,
                ["url"] = video.Link,
                ["embedUrl"] = embedUrl,
                ["thumbnailUrl"] = string.IsNullOrWhiteSpace(video.Thumbnail)
                    ? "https://prime.uweb4.com/default-thumbnail.png"
                    : video.Thumbnail,
                ["author"] = new Dictionary<string, object?>
                {
                    ["@type"] = "Person",
                    ["name"] = authorName
                },
                ["isPremium"] = video.IsPremium ?? false,
                ["likes"] = video.LikesCount,
                ["comments"] = video.CommentsCount
            };

            return nlwebJson;
        }
    }
}