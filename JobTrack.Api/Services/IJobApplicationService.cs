using JobTrack.Api.DTOs;
using JobTrack.Api.Enums;

namespace JobTrack.Api.Services;

public interface IJobApplicationService
{
    Task<List<JobApplicationResponseDto>> GetAllApplicationsAsync(ApplicationStatus? statusFilter = null,string? searchTerm = null,int pageNumber = 1, int pageSize = 10);
    Task<JobApplicationResponseDto?> GetApplicationByIdAsync(int id);
    Task<(JobApplicationResponseDto? CreatedDto, string? Error)> CreateApplicationAsync(CreateJobApplicationDto dto);
    Task<(bool IsNotFound, string? Error)> UpdateApplicationAsync(int id, UpdatedJobApplicationDto dto);
    Task<(bool IsNotFound, string? Error)> DeleteApplicationAsync(int id);
}