using uweb4Media.Application.Dtos;
using Uweb4Media.Domain.Entities.Admin.Video;

namespace uweb4Media.Application.Features.CQRS.Results.Admin.Video;

public class GetVideoByIdQueryResult
{
    public int Id { get; set; }
    public string Link { get; set; }
     
    public List<VideoLocalizedDataResultDto> LocalizedData { get; set; } 
    
    public string Thumbnail { get; set; }
    public List<string> Sector { get; set; }
    public List<string> Channel { get; set; }
    public string ContentType { get; set; }
    public string PublishStatus { get; set; } 
    public List<string> Tags { get; set; }
    public DateTime? Date { get; set; }
    public string Responsible { get; set; }
    public int? CompanyId { get; set; }
    public bool IsPremium { get; set; }  
    public int LikesCount { get; set; }  
    public int CommentsCount { get; set; }
    public int? UserId { get; set; }
} 

