using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata.Ecma335;

namespace Uweb4Media.Domain.Entities;

public class Notification
{
    [Key]
    public int Id { get; set; }  
 
    [Required]
    public int UserId { get; set; }

    [ForeignKey("UserId")]
    public AppUser User { get; set; }  

    [Required]
    public string Message { get; set; }  

    [Required]
    [MaxLength(50)] 
    public string Type { get; set; }  

    [Required]
    public bool IsRead { get; set; } = false;  

    [Required]
    public DateTime NotificationDate { get; set; } = DateTime.UtcNow; 
}