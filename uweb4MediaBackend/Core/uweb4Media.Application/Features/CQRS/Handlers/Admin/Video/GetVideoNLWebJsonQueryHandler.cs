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

            // Schema.org type belirle
            var schemaType = (video.ContentType ?? "").ToLower() switch
            {
                "podcast" => "PodcastEpisode",
                "video" => "VideoObject",
                _ => "Article"
            };

            // Publisher adını belirle
            string publisherName = !string.IsNullOrWhiteSpace(video.Responsible)
                ? video.Responsible!
                : (video.Company?.Name ?? "Unknown");

            // Author adını belirle
            string authorName = !string.IsNullOrWhiteSpace(video.Responsible)
                ? video.Responsible!
                : (video.User?.Username ?? "Unknown");

            // Tagler null ise boş array ata
            var tags = video.Tags ?? new List<string>();

            // ISO 8601 tarih formatı
            string? datePublished = video.Date?.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");

            // Doğru JSON-LD key'leri için Dictionary kullan!
            var nlwebJson = new Dictionary<string, object?>
            {
                ["@context"] = "https://schema.org",
                ["@type"] = schemaType,
                ["headline"] = video.Title,
                ["name"] = video.Title,
                ["publisher"] = new Dictionary<string, object?>
                {
                    ["@type"] = "Organization",
                    ["name"] = publisherName
                },
                ["datePublished"] = datePublished,
                ["uploadDate"] = datePublished,
                ["description"] = video.Description,
                ["keywords"] = tags,
                ["url"] = $"https://prime.uweb4.com/content/{video.Id}",
                ["embedUrl"] = video.Link,
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