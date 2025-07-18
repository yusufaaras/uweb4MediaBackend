using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Uweb4Media.Domain.Entities;

public class Comment
{
    [Key]
    public int Id { get; set; } 
    [Required]
    public string Text { get; set; } 
    public DateTime CommentDate { get; set; } = DateTime.UtcNow; 
    public int UserId { get; set; } 
    [ForeignKey("UserId")]
    public AppUser User { get; set; } 
    public int MediaContentId { get; set; } 
    [ForeignKey("MediaContentId")]
    public MediaContent MediaContent { get; set; } 
}