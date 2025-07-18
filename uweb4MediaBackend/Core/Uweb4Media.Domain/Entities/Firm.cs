namespace Uweb4Media.Domain.Entities;

public class Firm
{
    public int Id { get; set; }
    public string FirmName { get; set; }
    public string WebSiteUrl { get; set; }
    public string Sector { get; set; }
    public string Country  { get; set; }
    public string AuthorizedPerson  { get; set; }
    public string AuthorizedPersonEmail  { get; set; }
    public string LogoUrl { get; set; }
    public bool status  { get; set; }=false;
    public AppUser User { get; set; }
    public int UserId { get; set; }
}