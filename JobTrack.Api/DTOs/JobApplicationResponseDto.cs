using JobTrack.Api.Enums;

namespace JobTrack.Api.DTOs;

public class JobApplicationResponseDto
{
    public int Id { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public ApplicationStatus Status { get; set; } = ApplicationStatus.Applied;

    public string Notes { get; set; } = string.Empty;

    public DateTime AppliedDates { get; set; }
}