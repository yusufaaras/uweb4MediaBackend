using uweb4Media.Application.Dtos;
using uweb4Media.Application.Features.CQRS.Results.Admin.Video;
using uweb4Media.Application.Interfaces;
using Microsoft.EntityFrameworkCore; 

namespace uweb4Media.Application.Features.CQRS.Handlers.Admin.Video;

public class GetVideoQueryHandler
{
    private readonly IRepository<Uweb4Media.Domain.Entities.Admin.Video.Video> _repository;

    public GetVideoQueryHandler(IRepository<Uweb4Media.Domain.Entities.Admin.Video.Video> repository)
    {
        _repository = repository;
    }
    
    public async Task<List<GetVideoQueryResult>> Handle()
    {
        // ✅ LocalizedStrings'leri dahil et
        var values = await _repository.GetAllWithIncludesAsync(x => x.LocalizedStrings);
    
        // DEBUG: İlk video'nun LocalizedStrings'ini kontrol et
        var firstVideo = values.FirstOrDefault();
        if (firstVideo != null)
        {
            Console.WriteLine($"🎬 First video LocalizedStrings count: {firstVideo.LocalizedStrings?.Count ?? 0}");
            if (firstVideo.LocalizedStrings?.Any() == true)
            {
                var firstLocalized = firstVideo.LocalizedStrings.First();
                Console.WriteLine($"🎬 First localized: Lang={firstLocalized.LanguageCode}, Title={firstLocalized.Title}");
            }
        }
    
        return values.Select(x => new GetVideoQueryResult
        { 
            Id = x.Id,
            Link = x.Link, 
            Thumbnail = x.Thumbnail,
            Sector = x.Sector,
            Channel = x.Channel,
            ContentType = x.ContentType,
            PublishStatus = x.PublishStatus,
            PublishDate = x.PublishDate,
            Tags = x.Tags,
            Date = x.Date,
            Responsible = x.Responsible,
            CompanyId = x.CompanyId,
            LocalizedData = x.LocalizedStrings?.Select(ls => new VideoLocalizedDataResultDto
            {
                LanguageCode = ls.LanguageCode,
                Title = ls.Title,
                Description = ls.Description
            }).ToList() ?? new List<VideoLocalizedDataResultDto>()
        }).ToList();
    }
}