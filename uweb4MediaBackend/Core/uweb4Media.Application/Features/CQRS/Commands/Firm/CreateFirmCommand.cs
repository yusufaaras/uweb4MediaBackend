namespace uweb4Media.Application.Features.CQRS.Commands.Firm;

public class CreateFirmCommand
{  
    public string FirmName { get; set; }
    public string WebSiteUrl { get; set; }
    public string Sector { get; set; }
    public string Country  { get; set; }
    public string LogoUrl { get; set; }
    public string AuthorizedPerson  { get; set; }
    public string AuthorizedPersonEmail  { get; set; }
    public int UserId { get; set; }
}