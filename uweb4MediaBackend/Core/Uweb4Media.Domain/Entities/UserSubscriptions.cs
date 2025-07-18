using uweb4Media.Application.Enums;

namespace Uweb4Media.Domain.Entities;

public class UserSubscriptions
{
    public int Id { get; set; }

    // Kullanıcı ve plan ilişkileri
    public int UserId { get; set; }
    public AppUser AppUser { get; set; }

    public int PlansId { get; set; }
    public Plans Plans { get; set; }

    // Ödeme tarihleri
    public DateTime StartDate { get; set; }              // Abonelik başlangıç tarihi
    public DateTime? FinalPayment { get; set; }          // Son ödeme yapılan tarih (opsiyonel)
    public DateTime? NextPayment { get; set; }           // Bir sonraki ödeme tarihi

    // Abonelik durumu
    public SubscriptionStatus Status { get; set; } = SubscriptionStatus.Active;

    // Otomatik yenileme seçeneği (opsiyonel)
    public bool AutoRenew { get; set; } = true;
    public int DurationInDays { get; set; }              // Plan süresi (backend hesaplama için yardımcı olur)
}
