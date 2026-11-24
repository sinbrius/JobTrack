using JobTrack.Api.Models;
using JobTrack.Api.Data;
using Microsoft.EntityFrameworkCore;
using JobTrack.Api.Validators;
using JobTrack.Api.DTOs;
using JobTrack.Api.Enums;

namespace JobTrack.Api.Services;
public class JobApplicationService : IJobApplicationService
{
    private readonly AppDbContext _dbContext;
    private readonly JobApplicationValidator _validator ;

    public JobApplicationService(AppDbContext dbContext, JobApplicationValidator validator)
    {
        _dbContext = dbContext;
        _validator = validator;
    }

    public async Task<List<JobApplicationResponseDto>> GetAllApplicationsAsync(ApplicationStatus? statusFilter = null,string? searchTerm = null, int pageNumber = 1, int pageSize = 10)
    {
        var query = _dbContext.JobApplications.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(a => a.CompanyName.Contains(searchTerm.ToLower()) || a.Position.Contains(searchTerm.ToLower()));
        }

        if (statusFilter.HasValue)
        {
            query = query.Where(a => a.Status == statusFilter.Value);
        }
        

        var applications = await query
        .OrderByDescending(a => a.AppliedDate) //En yeni başvuruların en üstte görünmesi için AppliedDates'e göre sıralama
        .Skip((pageNumber-1)*pageSize)
        .Take(pageSize)
        .ToListAsync();

        return applications.Select(a => MapToResponseDto(a)).ToList();
    }
    public async Task<JobApplicationResponseDto?> GetApplicationByIdAsync(int id)
    {
        var application = await _dbContext.JobApplications
            .Where(a => a.Id == id)
            .FirstOrDefaultAsync();

        if (application is null)
            return null;

        return MapToResponseDto(application);
    }
    public async Task<(JobApplicationResponseDto? CreatedDto, string? Error)> CreateApplicationAsync(CreateJobApplicationDto dto)
    {
        var validationError = _validator.Validate(dto);
        if (validationError != null)
        {
            return (null, validationError);
        }

        var application = new JobApplication
        {
            CompanyName = dto.CompanyName,
            Position = dto.Position,
            Location = dto.Location,
            Status = dto.Status,
            Notes = dto.Notes
        };

        _dbContext.JobApplications.Add(application);
        await _dbContext.SaveChangesAsync();

        return (MapToResponseDto(application), null);
    }
    public async Task<(bool IsNotFound, string? Error)> UpdateApplicationAsync(int id, UpdatedJobApplicationDto dto)
    {
        var application = await _dbContext.JobApplications.FindAsync(id);
        if (application is null)
        {
            return (true, null);
        }

        var validationError = _validator.Validate(new CreateJobApplicationDto
        {
            CompanyName = dto.CompanyName,
            Position = dto.Position,
            Location = dto.Location,
            Status = dto.Status,
            Notes = dto.Notes
        });

        if (validationError != null)
        {
            return (false, validationError);
        }

        application.CompanyName = dto.CompanyName;
        application.Position = dto.Position;
        application.Location = dto.Location;
        application.Status = dto.Status;
        application.Notes = dto.Notes;

        await _dbContext.SaveChangesAsync();

        return (false, null);
    }
    public async Task<(bool IsNotFound, string? Error)> DeleteApplicationAsync(int id)
    {
        var application = await _dbContext.JobApplications.FindAsync(id);
        if (application is null)
        {
            return (true, null);
        }

        _dbContext.JobApplications.Remove(application);
        await _dbContext.SaveChangesAsync();

        return (false, null);
    }

    private JobApplicationResponseDto MapToResponseDto(JobApplication application)
    {
        return new JobApplicationResponseDto
        {
            Id = application.Id,
            CompanyName = application.CompanyName,
            Position = application.Position,
            Location = application.Location,
            Status = application.Status,
            Notes = application.Notes,
            AppliedDates = application.AppliedDates
        };
    }
}