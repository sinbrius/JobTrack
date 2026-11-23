using System.ComponentModel.DataAnnotations;
using JobTrack.Api.Enums;

namespace JobTrack.Api.DTOs;
public class CreateJobApplicationDto
{
    [Required]
    public string CompanyName { get; set; } = string.Empty;
    [Required]
    public string Position { get; set; } = string.Empty;
    [Required]
    public string Location { get; set; } = string.Empty;
    public ApplicationStatus Status { get; set; } = ApplicationStatus.Applied;

    public string Notes { get; set; } = string.Empty;
    
}