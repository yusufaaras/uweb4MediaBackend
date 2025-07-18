namespace uweb4Media.Application.Features.CQRS.Queries.Comments;

public class GetCommentsByMediaContentIdQuery
{
    public int MediaContentId { get; set; }

    public GetCommentsByMediaContentIdQuery(int mediaContentId)
    {
        MediaContentId = mediaContentId;
    }
}