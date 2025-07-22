using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Uweb4Media.Domain.Entities.Admin.CompanyManagement;

namespace Uweb4Media.Domain.Entities.Admin.Video
{
    public class Video
    {
        [Key] 
        public Guid Id { get; set; } 

        [Required]  
        public string Link { get; set; } 
        public ICollection<VideoLocalizedString> LocalizedStrings { get; set; } = new List<VideoLocalizedString>();

        public string Thumbnail { get; set; } 
        public List<string> Sector { get; set; } = new List<string>();
        public List<string> Channel { get; set; } = new List<string>();

        public string ContentType { get; set; } // 'Video', 'Podcast' vb. (tercihen enum)
        public string PublishStatus { get; set; } // 'Aktif', 'İncelemede' vb. (tercihen enum)

        public DateTime? PublishDate { get; set; } // Opsiyonel olabilir (DateTime tipi)

        public List<string> Tags { get; set; } = new List<string>(); // Yine JSON olarak saklanacak

        public DateTime Date { get; set; } // Genellikle oluşturulma tarihi
        public string Responsible { get; set; }

        public Guid? CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        public Company Company { get; set; } // Company tablosuna dış anahtar ilişkisi
    }

}