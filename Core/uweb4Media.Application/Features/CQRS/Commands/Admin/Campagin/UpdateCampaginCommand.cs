using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Uweb4Media.Application.Features.CQRS.Commands.Admin.Campaign
{
    public class UpdateCampaignCommand
    {
        [Required]
        public int Id { get; set; } 

        [Required]
        public string Name { get; set; }

        [Required]
        public int CompanyId { get; set; }

        public decimal Budget { get; set; }

        [Required]
        public string Status { get; set; } // Enum string olarak alınacak

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public List<string> Sectors { get; set; } = new();
        public List<string> Channels { get; set; } = new();

        public string Region { get; set; }
        public string AdFormat { get; set; }
    }
}