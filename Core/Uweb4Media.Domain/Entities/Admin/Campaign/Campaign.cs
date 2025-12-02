using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Uweb4Media.Domain.Entities.Admin.CompanyManagement;
using Uweb4Media.Domain.Enums;

namespace Uweb4Media.Domain.Entities.Admin.Campaign
{
    public class Campaign
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int CompanyId { get; set; }

        [ForeignKey("CompanyId")]
        public Company Company { get; set; }

        public decimal Budget { get; set; }

        public CampaignStatus Status { get; set; } = CampaignStatus.Planned;

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // Çoklu seçimler için
        public List<string> Sectors { get; set; } = new(); // JSON olarak saklanmalı
        public List<string> Channels { get; set; } = new(); // JSON olarak saklanmalı

        public string Region { get; set; }
        public string AdFormat { get; set; }

        public CampaignPerformance Performance { get; set; }
    }
}