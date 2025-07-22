namespace uweb4Media.Application.Features.CQRS.Results.Admin.Campaign;

public class GetCampaignByIdQueryResult
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int CompanyId { get; set; }
    public decimal Budget { get; set; }
    public string Status { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public List<string> Sectors { get; set; }
    public List<string> Channels { get; set; }
    public string Region { get; set; }
    public string AdFormat { get; set; } 
}