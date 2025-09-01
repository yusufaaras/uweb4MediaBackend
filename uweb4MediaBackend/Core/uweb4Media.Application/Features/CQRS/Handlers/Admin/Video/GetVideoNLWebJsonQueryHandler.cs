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

            var nlwebJson = new
            {
                @context = "https://schema.org",
                @type = schemaType,
                headline = video.Title,
                name = video.Title,
                publisher = new
                {
                    @type = "Organization",
                    name = publisherName
                },
                datePublished = datePublished, // optional
                uploadDate = datePublished,    // ZORUNLU! (datePublished ile aynı olabilir)
                description = video.Description,
                keywords = tags, 
                url = $"https://prime.uweb4.com/content/{video.Id}",
                embedUrl = video.Link,         // ZORUNLU/Youtube ise
                thumbnailUrl = video.Thumbnail, // ZORUNLU
                author = new
                {
                    @type = "Person",
                    name = authorName
                },
                isPremium = video.IsPremium ?? false,
                likes = video.LikesCount,
                comments = video.CommentsCount
            };
            
            
            
            return nlwebJson;
        }
    }
}