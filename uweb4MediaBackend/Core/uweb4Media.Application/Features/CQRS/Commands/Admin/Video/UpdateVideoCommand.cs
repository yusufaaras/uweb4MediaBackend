using Uweb4Media.Domain.Entities.Admin.Video;

namespace uweb4Media.Application.Features.CQRS.Commands.Admin.Video;

public class UpdateVideoCommand 
{
    public int Id { get; set; }

    public string? Link { get; set; }

    public List<VideoLocalizedDataDto>? LocalizedData { get; set; }

    public string? Thumbnail { get; set; }

    public List<string>? Sector { get; set; }
    public List<string>? Channel { get; set; }

    public string? ContentType { get; set; }
    public string? PublishStatus { get; set; } 
    public List<string>? Tags { get; set; } 
    public string? Responsible { get; set; }  
}        
