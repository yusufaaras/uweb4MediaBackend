namespace uweb4Media.Application.Features.CQRS.Queries.Admin.Campaign;

public class GetCampaignByIdQuery
{
    public GetCampaignByIdQuery(int id)
    {
        Id = id;
    }
    public int Id { get; set; }
}