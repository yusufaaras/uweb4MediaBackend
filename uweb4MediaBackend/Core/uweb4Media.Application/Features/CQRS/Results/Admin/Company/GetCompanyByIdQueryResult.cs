namespace uweb4Media.Application.Features.CQRS.Results.Company;

public class GetCompanyByIdQueryResult
{
    public int Id { get; set; }
    public string Name { get; set; }

    public string Logo { get; set; }
    public string Sector { get; set; }
    public string Country { get; set; }

    public string ContactPerson { get; set; }
    public string Email { get; set; }
    public string Website { get; set; }
    public string Status { get; set; } // Enum string ("Active","UnderReview","Passive")

    public DateTime? RegistrationDate { get; set; }
    public int ActiveCampaigns { get; set; }
    public decimal TotalSpend { get; set; }
    public int ContentUploaded { get; set; }
}