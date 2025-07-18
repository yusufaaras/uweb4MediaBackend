namespace uweb4Media.Application.Features.CQRS.Queries.Plans;

public class GetPlansByIdQuery
{
    public GetPlansByIdQuery(int id)
    {
        Id = id;
    }
    public int Id { get; set; }
}