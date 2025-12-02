namespace uweb4Media.Application.Features.CQRS.Queries.Comments;

public class GetCommentsByVideoIdQuery
{
    public int VideoId { get; set; }

    public GetCommentsByVideoIdQuery(int videoId)
    {
        VideoId = videoId;
    }
}