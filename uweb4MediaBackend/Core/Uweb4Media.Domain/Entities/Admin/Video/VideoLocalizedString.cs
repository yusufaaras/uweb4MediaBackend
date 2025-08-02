using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Uweb4Media.Domain.Entities.Admin.Video;

public class VideoLocalizedString
{
    [Key]  
    public int Id { get; set; }  

    [Required]
    public int VideoId { get; set; } 

    [ForeignKey("VideoId")]  
    public Video Video { get; set; }  
    public string Title { get; set; }  
    public string Description { get; set; }  
    
}