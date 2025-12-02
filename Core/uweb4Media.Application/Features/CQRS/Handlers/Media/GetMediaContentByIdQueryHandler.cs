using uweb4Media.Application.Features.CQRS.Queries.Media;
using uweb4Media.Application.Interfaces;
using Uweb4Media.Domain.Entities;

namespace uweb4Media.Application.Features.CQRS.Handlers.Media;

public class GetMediaContentByIdQueryHandler
{
    private readonly IRepository<MediaContent> _repository;
    public GetMediaContentByIdQueryHandler(IRepository<MediaContent> repository)
    {
        _repository = repository;
    }
    public async Task<GetMediaContentByIdQueryResult> Handle(GetMediaContentByIdQuery query)
    {
        var values = await _repository.GetByIdAsync(query.Id);
        return new GetMediaContentByIdQueryResult
        {
            Id = values.Id,
            UserId = values.UserId,
            Url = values.Url,
            Title = values.Title,
            Sector = values.Sector,
            Channel = values.Channel,
            ContentType = values.ContentType,
            Thumbnail = values.Thumbnail,
            Likes = values.LikesCount, 
            CommentsCount = values.CommentsCount, 
            Timestamp = values.CreatedDate, 
            ViewCount = values.ViewCount, 
            IsPremium = values.IsPremium, 
            MetaTitle = values.MetaTitle, 
            MetaDescription = values.MetaDescription, 
            Duration = values.Duration, 
            Excerpt = values.Excerpt, 
            YoutubeVideoId = values.YoutubeVideoId, 
            Tags = values.Tags 
            
        };
    }
}