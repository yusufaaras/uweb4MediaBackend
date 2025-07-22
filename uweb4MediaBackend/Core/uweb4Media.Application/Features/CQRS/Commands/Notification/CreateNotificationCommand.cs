using System.ComponentModel.DataAnnotations;

namespace uweb4Media.Application.Features.CQRS.Commands.Notification;

public class CreateNotificationCommand
{
    [Required]
    public Guid UserId { get; set; } 

    [Required]
    public string Message { get; set; }  

    [Required]
    [MaxLength(50)]  
    public string Type { get; set; }  

    public bool IsRead { get; set; } = false;  
    public DateTime? NotificationDate { get; set; }
}