using System.ComponentModel.DataAnnotations;
using Uweb4Media.Domain.Entities.Admin.Video;

namespace uweb4Media.Application.Features.CQRS.Commands.Admin.Video;

public class CreateVideoCommand
{
    [Required]
    public string Link { get; set; }

    
    public string Title { get; set; }  
    public string Description { get; set; }  
    
    public string? Thumbnail { get; set; }
    public string? Sector { get; set; }
    public string? Channel { get; set; }
    public string? ContentType { get; set; }
    public string? PublishStatus { get; set; } = "Incelemede";
    public List<string>? Tags { get; set; }
    public DateTime? Date { get; set; }
    public string? Responsible { get; set; }
    public bool? IsPremium { get; set; }
    public int? LikesCount { get; set; }
    public int? CommentsCount { get; set; }
    public int? UserId { get; set; }
    public int? CompanyId { get; set; }
}