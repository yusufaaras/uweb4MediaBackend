using uweb4Media.Application.Features.CQRS.Queries.Comments;
using uweb4Media.Application.Features.CQRS.Results.Comments;
using uweb4Media.Application.Interfaces;
using Uweb4Media.Domain.Entities;

namespace uweb4Media.Application.Features.CQRS.Handlers.Comments;

public class GetCommentByIdQueryHandler
{
    private readonly IRepository<Comment> _repository;
    public GetCommentByIdQueryHandler(IRepository<Comment> repository)
    {
        _repository = repository;
    }
    public async Task<GetCommentByIdQueryResult> Handle(GetCommentByIdQuery query)
    {
        var values = await _repository.GetByIdAsync(query.Id);
        return new GetCommentByIdQueryResult
        {
            Id = values.Id,
            UserId = values.UserId,
            Text = values.Text, 
            VideoId = values.VideoId,
            CommentDate = values.CommentDate,
        };
    }
}