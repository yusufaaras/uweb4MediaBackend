using uweb4Media.Application.Features.CQRS.Results.Comments;
using uweb4Media.Application.Interfaces;
using Uweb4Media.Domain.Entities;
using System.Linq;
using uweb4Media.Application.Features.CQRS.Queries.Comments; // Where metodunu kullanmak için gerekli

namespace uweb4Media.Application.Features.CQRS.Handlers.Comments;

public class GetCommentsByVideoIdQueryHandler
{
    private readonly IRepository<Comment> _repository;

    public GetCommentsByVideoIdQueryHandler(IRepository<Comment> repository)
    {
        _repository = repository;
    }

    public async Task<List<GetCommentQueryResult>> Handle(GetCommentsByVideoIdQuery query)
    {
        // Medya içerik ID'sine göre yorumları filtrele
        var values = (await _repository.GetAllAsync())
            .Where(x => x.VideoId == query.VideoId)
            .ToList(); 
        return values.Select(x => new GetCommentQueryResult
        {
            Id = x.Id,
            UserId = x.UserId,
            Text = x.Text,
            CommentDate = x.CommentDate,
            VideoId = x.VideoId,
        }).ToList();
    }
}