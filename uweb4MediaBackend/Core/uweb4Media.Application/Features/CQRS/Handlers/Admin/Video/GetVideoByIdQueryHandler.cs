using uweb4Media.Application.Dtos;
using uweb4Media.Application.Features.CQRS.Queries.Admin.Video;
using uweb4Media.Application.Features.CQRS.Results.Admin.Video;
using uweb4Media.Application.Interfaces;

namespace uweb4Media.Application.Features.CQRS.Handlers.Admin.Video;

public class GetVideoByIdQueryHandler
{
    private readonly IRepository<Uweb4Media.Domain.Entities.Admin.Video.Video> _repository;
    public GetVideoByIdQueryHandler(IRepository<Uweb4Media.Domain.Entities.Admin.Video.Video> repository)
    {
        _repository = repository;
    }
    public async Task<GetVideoByIdQueryResult> Handle(GetVideoByIdQuery query)
    {
        var values = await _repository.GetByIdAsync(query.Id);
        return new GetVideoByIdQueryResult
        {
            Id = values.Id,
            Link = values.Link, 
            Thumbnail = values.Thumbnail,
            Sector = values.Sector,
            Channel = values.Channel,
            ContentType = values.ContentType,
            PublishStatus = values.PublishStatus,
            PublishDate = values.PublishDate,
            Tags = values.Tags,
            Date = values.Date,
            Responsible = values.Responsible,
            CompanyId = values.CompanyId,
            LocalizedData = values.LocalizedStrings.Select(ls => new VideoLocalizedDataResultDto
            {
            LanguageCode = ls.LanguageCode,
            Title = ls.Title,
            Description = ls.Description
        }).ToList()
        };
    }
}