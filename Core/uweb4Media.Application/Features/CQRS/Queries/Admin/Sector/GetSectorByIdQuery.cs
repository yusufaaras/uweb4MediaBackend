namespace uweb4Media.Application.Features.CQRS.Queries.Admin.Sector;

public class GetSectorByIdQuery
{
    public GetSectorByIdQuery(int id)
    {
        Id = id;
    }
    public int Id { get; set; }
}