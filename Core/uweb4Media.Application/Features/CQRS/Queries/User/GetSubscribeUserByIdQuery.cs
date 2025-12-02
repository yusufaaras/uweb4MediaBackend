namespace uweb4Media.Application.Features.CQRS.Queries.User;

public class GetSubscribeUserByIdQuery
{
    public int Id { get; set; }
    public GetSubscribeUserByIdQuery(int id)
    {
        Id = id;
    }
}