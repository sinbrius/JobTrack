using JobTrack.Api.DTOs;
using JobTrack.Api.Services;
using JobTrack.Api.Enums;

namespace JobTrack.Api.Endpoints;

public static class JobApplicationEndpoints
{
    // "this WebApplication app" ifadesi bu metodu bir Extension Method yapar
    public static void MapJobApplicationEndpoints(this WebApplication app)
    {
        // Tüm rotaların ortak ön ekini tek yerde tanımlıyoruz
        var group = app.MapGroup("/api/applications")
                       .WithTags("Job Applications"); // Swagger'da başlık altında gruplar

        group.MapGet("/", async (IJobApplicationService jobApplicationService, ApplicationStatus? statusFilter,string? search,int page=1,int pageSize=10) =>
        {
            if(page < 1) page=1;
            if(pageSize < 1 || pageSize >50)  pageSize=10;
            var applications = await jobApplicationService.GetAllApplicationsAsync(statusFilter,search,page,pageSize);
            return Results.Ok(applications);
        })
        .WithName("GetJobTracks");

        group.MapGet("/{id}", async (int id, IJobApplicationService jobApplicationService) =>
        {
            var application = await jobApplicationService.GetApplicationByIdAsync(id);
            if (application is null)
                return Results.NotFound();

            return Results.Ok(application);
        })
        .WithName("GetJobApplication");

        group.MapPost("/", async (CreateJobApplicationDto dto, IJobApplicationService jobApplicationService) =>
        {
            var (createdDto, error) = await jobApplicationService.CreateApplicationAsync(dto);
            if (error is not null)
                return Results.BadRequest(error);

            return Results.Created($"/api/applications/{createdDto!.Id}", createdDto);
        })
        .WithName("CreateJobApplication");

        group.MapPut("/{id}", async (int id, UpdatedJobApplicationDto dto, IJobApplicationService jobApplicationService) =>
        {
            var (isNotFound, error) = await jobApplicationService.UpdateApplicationAsync(id, dto);
            if (isNotFound)
                return Results.NotFound();

            if (error is not null)
                return Results.BadRequest(error);

            return Results.NoContent();
        })
        .WithName("UpdateJobApplication");

        group.MapDelete("/{id}", async (int id, IJobApplicationService jobApplicationService) =>
        {
            var (isNotFound, _) = await jobApplicationService.DeleteApplicationAsync(id);
            if (isNotFound)
                return Results.NotFound();

            return Results.NoContent();
        })
        .WithName("DeleteJobApplication");
    }
}