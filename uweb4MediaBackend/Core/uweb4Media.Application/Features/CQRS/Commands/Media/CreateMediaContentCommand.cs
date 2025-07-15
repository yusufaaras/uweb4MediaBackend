namespace uweb4Media.Application.Features.CQRS.Commands.Media;

public class CreateMediaContentCommand
{
    public int UserId { get; set; } 
    public string Url { get; set; }
    public string Title { get; set; }
    public string Sector { get; set; }
    public string Channel { get; set; }
    public string ContentType { get; set; }
    public string Thumbnail { get; set; } 
}