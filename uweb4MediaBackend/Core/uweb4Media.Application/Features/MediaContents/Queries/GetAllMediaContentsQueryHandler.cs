using MediatR;
using Uweb4Media.Domain.Repositories;
using Uweb4Media.Application.Features.MediaContents.Dtos;

namespace Uweb4Media.Application.Features.MediaContents.Queries;

public class GetAllMediaContentsQueryHandler : IRequestHandler<GetAllMediaContentsQuery, List<MediaContentDto>>
{
    private readonly IMediaContentRepository _mediaRepo;

    public GetAllMediaContentsQueryHandler(IMediaContentRepository mediaRepo)
    {
        _mediaRepo = mediaRepo;
    }

    public async Task<List<MediaContentDto>> Handle(GetAllMediaContentsQuery request, CancellationToken cancellationToken)
    {
        var entities = await _mediaRepo.GetAllAsync();

        return entities.Select(e => new MediaContentDto
        {
            Id = e.Id,
            Title = e.Title,
            ThumbnailUrl = e.ThumbnailUrl,
            Sector = e.Sector.ToString(),
            Channel = e.Channel.ToString(),
            ContentType = e.ContentType.ToString(),
            YoutubeVideoId = e.YoutubeVideoId,
            UserId = e.UserId,
            CreatedAt = e.CreatedAt
        }).ToList();
    }
}