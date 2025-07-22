using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Uweb4Media.Domain.Entities.Admin.Campaign
{
    public class CampaignPerformance
    {
        [Key]
        public int Id { get; set; }

        public int Impressions { get; set; }

        [Required]
        public int CampaignId { get; set; }

        [ForeignKey("CampaignId")]
        public Campaign Campaign { get; set; }
    }
}