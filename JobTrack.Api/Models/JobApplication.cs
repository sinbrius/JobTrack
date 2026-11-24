using System.ComponentModel.DataAnnotations;
using JobTrack.Api.Enums;

namespace JobTrack.Api.Models
{
    public class JobApplication{
        public int Id{get; set;}
        [Required(ErrorMessage ="Company Name is required")]
        public string CompanyName { get; set; } = string.Empty;

        [Required(ErrorMessage ="Position is required")]
        public string Position { get; set; } = string.Empty;
        [Required(ErrorMessage ="Location is required")]        
        public string Location { get; set; } = string.Empty;
        
        public ApplicationStatus Status { get; set; } = ApplicationStatus.Applied;

        public string? Notes { get; set; }

        public DateTime AppliedDates { get; set; } = DateTime.UtcNow;
    }
}