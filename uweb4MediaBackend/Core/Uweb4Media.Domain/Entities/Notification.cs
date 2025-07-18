using System.Reflection.Metadata.Ecma335;

namespace Uweb4Media.Domain.Entities;

public class Notification
{
    public int Id { get; set; }
    public int UserId { get; set; } 
    public AppUser User { get; set; } 
    public string Text { get; set; }
    public DateTime NotificationDate { get; set; } = DateTime.UtcNow;

}