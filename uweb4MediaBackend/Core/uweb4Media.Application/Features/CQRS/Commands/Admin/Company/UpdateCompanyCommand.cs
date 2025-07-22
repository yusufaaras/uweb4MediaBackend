using System.ComponentModel.DataAnnotations;

namespace uweb4Media.Application.Features.CQRS.Commands.Admin.Company;

public class UpdateCompanyCommand
{
        [Required]
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

        [Required]
        public string Status { get; set; } // Enum string ("Active","UnderReview","Passive") 
        public int ActiveCampaigns { get; set; }
        public decimal TotalSpend { get; set; }
        public int ContentUploaded { get; set; }
}