using JobTrack.Api.DTOs;

namespace JobTrack.Api.Services;

public interface IJobApplicationService
{
    Task<List<JobApplicationResponseDto>> GetAllApplicationsAsync();
    Task<JobApplicationResponseDto?> GetApplicationByIdAsync(int id);
    Task<(JobApplicationResponseDto? CreatedDto, string? Error)> CreateApplicationAsync(CreateJobApplicationDto dto);
    Task<(bool IsNotFound, string? Error)> UpdateApplicationAsync(int id, UpdatedJobApplicationDto dto);
    Task<(bool IsNotFound, string? Error)> DeleteApplicationAsync(int id);
}