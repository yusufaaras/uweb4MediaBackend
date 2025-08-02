using Uweb4Media.Domain.Entities.Admin.Video;

namespace uweb4Media.Application.Features.CQRS.Commands.Admin.Video;

public class UpdateVideoCommand 
{
    public int Id { get; set; }

    public string? Link { get; set; }

    
    public string Title { get; set; }  
    public string Description { get; set; }  

    public string? Thumbnail { get; set; }

    public string? Sector { get; set; }
    public string? Channel { get; set; }

    public string? ContentType { get; set; }
    public string? PublishStatus { get; set; } 
    public List<string>? Tags { get; set; } 
    public string? Responsible { get; set; }  
}        
