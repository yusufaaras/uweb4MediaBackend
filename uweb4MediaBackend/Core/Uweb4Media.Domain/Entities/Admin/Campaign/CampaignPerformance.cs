using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Uweb4Media.Domain.Entities.Admin.Campaign
{
    public class CampaignPerformance
    {
        [Key]
        public Guid Id { get; set; }

        public int Impressions { get; set; }

        [Required]
        public Guid CampaignId { get; set; }

        [ForeignKey("CampaignId")]
        public Campaign Campaign { get; set; }
    }
}