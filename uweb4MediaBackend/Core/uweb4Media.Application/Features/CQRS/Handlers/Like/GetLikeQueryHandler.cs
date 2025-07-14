using uweb4Media.Application.Features.CQRS.Handlers.Media;
using uweb4Media.Application.Features.CQRS.Results.Like;
using uweb4Media.Application.Interfaces;
using Uweb4Media.Domain.Entities;

namespace uweb4Media.Application.Features.CQRS.Handlers.Like;

public class GetLikeQueryHandler
{ 
    private readonly IRepository<Uweb4Media.Domain.Entities.Like> _repository;

    public GetLikeQueryHandler(IRepository<Uweb4Media.Domain.Entities.Like> repository)
    {
        _repository = repository;
    }
    public async Task<List<GetLikeQueryResult>> Handle()
    {
        var values = await _repository.GetAllAsync();
        return values.Select(x => new GetLikeQueryResult
        {
            Id=x.Id,
            UserId = x.UserId,
            LikeDate = x.LikeDate,
            MediaContentId = x.MediaContentId,
        }).ToList();
    }
}