using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Uweb4Media.Domain.Entities.Admin.CompanyManagement;

namespace Uweb4Media.Domain.Entities.Admin.Video
{
    public class Video
    {
        [Key] public int Id { get; set; }

        [Required] public string Link { get; set; }

        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string Thumbnail { get; set; } = "";
        public string Sector { get; set; } = "";
        public string Channel { get; set; } = "";
        public string ContentType { get; set; } = "";
        public string? PublishStatus { get; set; } = "Incelemede";
        public List<string>? Tags { get; set; } = new List<string>();
        public DateTime? Date { get; set; }
        public string? Responsible { get; set; } = "";
        public bool? IsPremium { get; set; } = false;
        public int LikesCount { get; set; } = 0;
        public int CommentsCount { get; set; } = 0;
        public int? UserId { get; set; }
        [ForeignKey("UserId")] public AppUser? User { get; set; }
        public int? CompanyId { get; set; }
        [ForeignKey("CompanyId")] public Company? Company { get; set; }
        public ICollection<Like>? Likes { get; set; }
        public ICollection<Comment>? Comments { get; set; }
    }
}