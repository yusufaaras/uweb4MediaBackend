using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Uweb4Media.Domain.Entities.Admin.Video;

public class VideoLocalizedString
{
    [Key] // Bu entity'nin birincil anahtarı
    public Guid Id { get; set; } // int yerine Guid kullanmak daha iyi

    [Required]
    public Guid VideoId { get; set; } // Bu, hangi Video'ya ait olduğunu belirten dış anahtardır

    [ForeignKey("VideoId")] // Video tablosuna dış anahtar ilişkisi kurar
    public Video Video { get; set; } // Video entity'sine navigasyon özelliği

    [Required]
    [MaxLength(10)] // "tr", "en", "de" gibi dil kodları için yeterli uzunluk
    public string LanguageCode { get; set; } // Dil kodu (örn: "tr", "en", "de")

    public string Title { get; set; }  
    public string Description { get; set; }  
    
}