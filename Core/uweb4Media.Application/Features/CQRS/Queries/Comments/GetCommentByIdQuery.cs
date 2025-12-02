namespace uweb4Media.Application.Features.CQRS.Queries.Comments;

public class GetCommentByIdQuery
{
    public GetCommentByIdQuery(int id)
    {
        Id = id;
    }
    public int Id { get; set; }
}