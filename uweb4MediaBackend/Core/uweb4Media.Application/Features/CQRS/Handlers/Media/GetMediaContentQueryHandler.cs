using uweb4Media.Application.Interfaces;
using Uweb4Media.Domain.Entities;

namespace uweb4Media.Application.Features.CQRS.Handlers.Media;

public class GetMediaContentQueryHandler
{
    private readonly IRepository<MediaContent> _repository;

    public GetMediaContentQueryHandler(IRepository<MediaContent> repository)
    {
        _repository = repository;
    }
    public async Task<List<GetMediaContentQueryResult>> Handle()
    {
        var values = await _repository.GetAllAsync();
        return values.Select(x => new GetMediaContentQueryResult
        {
            Id = x.Id,
            UserId = x.UserId,
            Url = x.Url,
            Title = x.Title,
            ContentType = x.ContentType,
            Sector = x.Sector,
            Channel = x.Channel,
            Thumbnail = x.Thumbnail,
        }).ToList();
    }
} 