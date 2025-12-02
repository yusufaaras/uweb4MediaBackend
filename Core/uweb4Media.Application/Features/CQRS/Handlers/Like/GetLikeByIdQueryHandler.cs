using uweb4Media.Application.Features.CQRS.Queries.Like;
using uweb4Media.Application.Features.CQRS.Results.Like;
using uweb4Media.Application.Interfaces;

namespace uweb4Media.Application.Features.CQRS.Handlers.Like;
using Uweb4Media.Domain.Entities;

public class GetLikeByIdQueryHandler
{
    private readonly IRepository<Like> _repository;
    public GetLikeByIdQueryHandler(IRepository<Like> repository)
    {
        _repository = repository;
    }
    public async Task<GetLikeByIdQueryResult> Handle(GetLikeByIdQuery query)
    {
        var values = await _repository.GetByIdAsync(query.Id);
        return new GetLikeByIdQueryResult
        {
            Id = values.Id,
            UserId = values.UserId,
            VideoId = values.VideoId,
            LikeDate = values.LikeDate,
        };
    }
    
}