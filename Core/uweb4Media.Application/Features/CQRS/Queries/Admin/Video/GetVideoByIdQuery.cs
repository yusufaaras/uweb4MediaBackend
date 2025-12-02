namespace uweb4Media.Application.Features.CQRS.Queries.Admin.Video;

public class GetVideoByIdQuery
{
    public GetVideoByIdQuery(int id)
    {
        Id = id;
    }
    public int Id { get; set; }
}