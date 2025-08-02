using System.ComponentModel.DataAnnotations;
using Uweb4Media.Domain.Entities.Admin.Video;

namespace uweb4Media.Application.Features.CQRS.Commands.Admin.Video;

public class CreateVideoCommand
{
    [Required]
    public string Link { get; set; }

    [Required]
    public List<VideoLocalizedDataDto> LocalizedData { get; set; } = new();

    public string Thumbnail { get; set; }

    public List<string> Sector { get; set; } = new();
    public List<string> Channel { get; set; } = new();

    public string ContentType { get; set; }
    public string PublishStatus { get; set; } = "Incelemede";
    public List<string> Tags { get; set; } = new();

    public DateTime? Date { get; set; }
    public string Responsible { get; set; }
    public int? CompanyId { get; set; } 
        
    public bool IsPremium { get; set; } = false; 
        
    public int LikesCount { get; set; } = 0; 
    public int CommentsCount { get; set; } = 0;  

    public int? UserId { get; set; }
}

public class VideoLocalizedDataDto
{
    public int Id { get; set; }
    [Required]
    [MaxLength(10)] 

    public string Title { get; set; }
    public string Description { get; set; }
}