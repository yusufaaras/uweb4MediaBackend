using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Uweb4Media.Domain.Entities.Admin.CompanyManagement;

namespace Uweb4Media.Domain.Entities;

public class Subscription
{
    [Key]
    public int Id { get; set; }

    // Takip eden kullanıcı
    public int SubscriberUserId { get; set; }
    [ForeignKey("SubscriberUserId")]
    public AppUser Subscriber { get; set; }

    // Takip edilen kullanıcı (opsiyonel)
    public int? AuthorUserId { get; set; }
    [ForeignKey("AuthorUserId")]
    public AppUser Author { get; set; }

    // Takip edilen şirket (opsiyonel)
    public int? AuthorCompanyId { get; set; }
    [ForeignKey("AuthorCompanyId")]
    public Company AuthorCompany { get; set; }

    public DateTime SubscribedDate { get; set; } = DateTime.UtcNow;
}