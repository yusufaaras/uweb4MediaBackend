using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Uweb4Media.Domain.Enums;

namespace Uweb4Media.Domain.Entities.Admin.CompanyManagement
{
    public class Company
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Logo { get; set; }

        [Required]
        public string Sector { get; set; }

        [Required]
        public string Country { get; set; }

        public string ContactPerson { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }

        public CompanyStatus Status { get; set; } = CompanyStatus.UnderReview;

        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
        public int ActiveCampaigns { get; set; }
        public decimal TotalSpend { get; set; }
        public int ContentUploaded { get; set; }

        // Navigation Properties
        public ICollection<Uweb4Media.Domain.Entities.Admin.Campaign.Campaign> Campaigns { get; set; }
        public ICollection<Uweb4Media.Domain.Entities.Admin.Video.Video> Videos { get; set; }
    }
}