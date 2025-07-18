namespace uweb4Media.Application.Features.CQRS.Queries.Firm;

public class GetFirmByIdQuery
{
    public GetFirmByIdQuery(int id)
    {
        Id = id;
    }
    public int Id { get; set; }
}