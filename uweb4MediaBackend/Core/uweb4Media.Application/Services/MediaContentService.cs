using Uweb4Media.Domain.Entities;
using Uweb4Media.Domain.Enums;
using Uweb4Media.Domain.Repositories;

namespace Uweb4Media.Application.Services
{
    public class MediaContentService
    {
        private readonly IMediaContentRepository _mediaRepo;

        public MediaContentService(IMediaContentRepository mediaRepo)
        {
            _mediaRepo = mediaRepo;
        }

        public async Task CreateMediaContentAsync(string title, string thumbnailUrl, Sector sector, Channel channel, ContentType type, string? youtubeVideoId, Guid userId)
        {
            var media = new MediaContent
            {
                Title = title,
                ThumbnailUrl = thumbnailUrl,
                Sector = sector,
                Channel = channel,
                ContentType = type,
                YoutubeVideoId = youtubeVideoId,
                UserId = userId
            };

            await _mediaRepo.AddAsync(media);
        }
        
        public async Task<List<MediaContent>> GetAllMediaContentsAsync()
        {
            return await _mediaRepo.GetAllAsync();
        }

    }
}