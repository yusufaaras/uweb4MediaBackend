using uweb4Media.Application.Features.CQRS.Queries.Admin.Video;
using uweb4Media.Application.Features.CQRS.Results.Admin.Video;
using uweb4Media.Application.Interfaces;

namespace uweb4Media.Application.Features.CQRS.Handlers.Admin.Video;

public class SearchVideoQueryHandler
{
    private readonly IRepository<Uweb4Media.Domain.Entities.Admin.Video.Video> _repository;

    public SearchVideoQueryHandler(IRepository<Uweb4Media.Domain.Entities.Admin.Video.Video> repository)
    {
        _repository = repository;
    }

    public async Task<List<GetVideoQueryResult>> Handle(SearchVideoQuery query)
    {
        var allVideos = await _repository.GetAllAsync();

        // Küçük-büyük harf duyarsız arama için
        var searchLower = query.SearchText?.ToLower() ?? "";

        var filtered = allVideos.Where(x =>
            (!string.IsNullOrEmpty(x.Title) && x.Title.ToLower().Contains(searchLower)) ||
            (!string.IsNullOrEmpty(x.Description) && x.Description.ToLower().Contains(searchLower)) ||
            (x.Tags != null && x.Tags.Any(tag => tag.ToLower().Contains(searchLower)))
        ).Select(x => new GetVideoQueryResult
        {
            Id = x.Id,
            Link = x.Link,
            Title = x.Title,
            Description = x.Description,
            Thumbnail = x.Thumbnail,
            Sector = x.Sector,
            Channel = x.Channel,
            ContentType = x.ContentType,
            PublishStatus = x.PublishStatus,
            IsPremium = x.IsPremium,
            LikesCount = x.LikesCount,
            CommentsCount = x.CommentsCount,
            UserId = x.UserId,
            Tags = x.Tags,
            Date = x.Date,
            Responsible = x.Responsible,
            CompanyId = x.CompanyId
        }).ToList();

        return filtered;
    }
}