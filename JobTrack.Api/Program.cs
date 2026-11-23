using Microsoft.OpenApi.Models;
using JobTrack.Api.Data;
using Microsoft.EntityFrameworkCore;
using JobTrack.Api.Validators;
using JobTrack.Api.Services ;
using JobTrack.Api.DTOs;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=jobtrack.db"));

builder.Services.AddScoped<JobApplicationValidator>();

builder.Services.AddProblemDetails(); //standart hata yanıtları için
builder.Services.AddScoped<IJobApplicationService, JobApplicationService>();
// Swagger servisleri
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
});
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseExceptionHandler(); //yakalanmamış istisnalar için hata işleyici

// Swagger yapılandırması
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/api/applications", async (IJobApplicationService jobApplicationService) =>
{
    var applications = await jobApplicationService.GetAllApplicationsAsync();
    return Results.Ok(applications);
})
.WithName("GetJobTracks");

app.MapGet("/api/applications/{id}", async (int id, IJobApplicationService jobApplicationService) =>
{
    var application = await jobApplicationService.GetApplicationByIdAsync(id);
    if (application is null)
    {
        return Results.NotFound();
    }

    return Results.Ok(application);
}).WithName("GetJobApplication");
    
app.MapPut("/api/applications/{id}", async (int id, UpdatedJobApplicationDto dto, IJobApplicationService jobApplicationService, JobApplicationValidator validator) =>
{
    var (isNotFound, error) = await jobApplicationService.UpdateApplicationAsync(id, dto);
    if (isNotFound)
    {
        return Results.NotFound();
    }
    if (error != null)
    {
        return Results.BadRequest(error);
    }
    return Results.NoContent();
}).WithName("UpdateJobApplication");
/**/


app.MapDelete("/api/applications/{id}", async (int id, IJobApplicationService jobApplicationService) =>
{
    var (isNotFound, error) = await jobApplicationService.DeleteApplicationAsync(id);
    if (isNotFound)
    {
        return Results.NotFound();
    }
    if (error != null)
    {
        return Results.BadRequest(error);
    }
    return Results.NoContent();
}).WithName("DeleteJobApplication");

app.MapPost("/api/applications", async (CreateJobApplicationDto dto, IJobApplicationService jobApplicationService, JobApplicationValidator validator) =>
{
    var (createdDto, error) = await jobApplicationService.CreateApplicationAsync(dto);
    if (error != null)
    {
        return Results.BadRequest(error);
    }
    return Results.Created($"/api/applications/{createdDto.Id}", createdDto);
}).WithName("CreateJobApplication");
/*Kodunun akışı şöyle:
1. DTO kullanıcıdan gelen JSON verisini alıyor.
2. validator.Validate(dto) verileri kontrol ediyor.
3. Hata varsa HTTP 400 dönüyor.
4. Hata yoksa DTO, JobApplication entity'sine dönüştürülüyor.
5. Entity veritabanına ekleniyor.
6. HTTP 201 dönüyor.*/


app.Run();

