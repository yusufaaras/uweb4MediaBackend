using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Uweb4Media.Application.Features.CQRS.Commands.Admin.Campaign
{
    public class CreateCampaignCommand
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public Guid CompanyId { get; set; }

        public decimal Budget { get; set; }

        [Required]
        public string Status { get; set; } // Enum string olarak alınacak (örn: "Planned")

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public List<string> Sectors { get; set; } = new();
        public List<string> Channels { get; set; } = new();

        public string Region { get; set; }
        public string AdFormat { get; set; }
    }
}