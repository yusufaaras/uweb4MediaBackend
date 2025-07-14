namespace uweb4Media.Application.Features.CQRS.Queries.Like;

public class GetLikeByIdQuery
{
    public GetLikeByIdQuery(int id)
    {
        Id = id;
    }
    public int Id { get; set; }
}