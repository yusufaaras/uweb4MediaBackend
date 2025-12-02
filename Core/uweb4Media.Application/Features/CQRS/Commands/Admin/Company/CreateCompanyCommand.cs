using System.ComponentModel.DataAnnotations;
using MediatR;
using Uweb4Media.Domain.Enums;

namespace uweb4Media.Application.Features.CQRS.Commands.Admin.Company;

public class CreateCompanyCommand
{
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

        // Frontend'den gönderilebilecek diğer alanlar:
        public DateTime? RegistrationDate { get; set; } // opsiyonel, genelde backend setler
        public int ActiveCampaigns { get; set; } // opsiyonel, genelde backend setler
        public decimal TotalSpend { get; set; } // opsiyonel, genelde backend setler
        public int ContentUploaded { get; set; } // opsiyonel, genelde backend setler
}