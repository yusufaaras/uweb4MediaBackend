using uweb4Media.Application.Features.CQRS.Results.Comments;
using uweb4Media.Application.Interfaces;
using Uweb4Media.Domain.Entities;

namespace uweb4Media.Application.Features.CQRS.Handlers.Comments;

public class GetCommentQueryHandler
{
    private readonly IRepository<Comment> _repository;

    public GetCommentQueryHandler(IRepository<Comment> repository)
    {
        _repository = repository;
    }
    public async Task<List<GetCommentQueryResult>> Handle()
    {
        var values = await _repository.GetAllAsync();
        return values.Select(x => new GetCommentQueryResult
        {
            Id=x.Id,
            UserId = x.UserId,
            Text = x.Text,
            CommentDate = x.CommentDate,
            MediaContentId = x.MediaContentId,
        }).ToList();
    }
}