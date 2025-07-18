using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Uweb4Media.Domain.Entities;

public class Subscription
{
    [Key]
    public int Id { get; set; }
    public int SubscriberUserId { get; set; }
    [ForeignKey("SubscriberUserId")]
    public AppUser Subscriber { get; set; } 
    public int AuthorUserId { get; set; }
    [ForeignKey("AuthorUserId")]
    public AppUser Author { get; set; } 
    public DateTime SubscribedDate { get; set; } = DateTime.UtcNow;
}