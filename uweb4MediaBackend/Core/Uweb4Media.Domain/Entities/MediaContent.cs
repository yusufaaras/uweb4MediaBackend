using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Uweb4Media.Domain.Entities;

public class MediaContent
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public AppUser User { get; set; } // İçeriği oluşturan kullanıcı

        [Required]
        public string Url { get; set; }

        [Required]
        [MaxLength(255)] // Başlık için max uzunluk
        public string Title { get; set; }
        [Required]
        [MaxLength(50)] // Sektör için max uzunluk
        public string Sector { get; set; } // Frontenddeki Sector enum'ı ile uyumlu olmalı
        [Required]
        [MaxLength(50)] // Kanal için max uzunluk
        public string Channel { get; set; } // Frontenddeki Channel enum'ı ile uyumlu olmalı
        [Required]
        [MaxLength(20)] // İçerik tipi için max uzunluk (Video, Podcast vb.)
        public string ContentType { get; set; }
        public string Thumbnail { get; set; } 
        public int LikesCount { get; set; } = 0; 
        public int CommentsCount { get; set; } = 0; 
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;  
        public int ViewCount { get; set; } = 0;
        public bool IsPremium { get; set; } = false;

        // SEO için meta başlık
        [MaxLength(255)]
        public string MetaTitle { get; set; }
        // SEO için meta açıklama
        public string MetaDescription { get; set; }
        // Video/Podcast süresi
        [MaxLength(20)]
        public string Duration { get; set; }
        // Blog/Haber için kısa özet (excerpt)
        public string Excerpt { get; set; }
        // YouTube videoları için özel ID (örneğin "B37KAveko2I")
        [MaxLength(50)]
        public string YoutubeVideoId { get; set; }
        // Etiketler için basit bir string (örn: "AI,Blockchain,IoT")
        // Daha karmaşık etiket yönetimi için ayrı bir Many-to-Many tablo düşünebilirsiniz.
        public string Tags { get; set; }
        public ICollection<Like> Likes { get; set; } // Bu içerikle ilişkili beğeniler
        public ICollection<Comment> Comments { get; set; } // Bu içerikle ilişkili yorumlar
    }