namespace uweb4Media.Application.Features.CQRS.Queries.Media;

public class GetMediaContentByIdQuery
{
    public GetMediaContentByIdQuery(int id)
    {
        Id = id;
    }
    public int Id { get; set; }
}