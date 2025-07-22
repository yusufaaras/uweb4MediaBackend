namespace uweb4Media.Application.Features.CQRS.Queries.Admin.Channel;

public class GetChannelByIdQuery
{
    public GetChannelByIdQuery(int id)
    {
        Id = id;
    }
    public int Id { get; set; }
}