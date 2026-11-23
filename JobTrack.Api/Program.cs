using Microsoft.OpenApi.Models;
using JobTrack.Api.Models;
using JobTrack.Api.Data;
using Microsoft.EntityFrameworkCore;
using JobTrack.Api.Validators;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=jobtrack.db"));

builder.Services.AddScoped<JobApplicationValidator>();

builder.Services.AddProblemDetails(); //standart hata yanıtları için
// Swagger servisleri
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
});
builder.Services.AddSwaggerGen();
builder .Services.AddProblemDetails();

var app = builder.Build();
app.UseExceptionHandler(); //yakalanmamış istisnalar için hata işleyici

// Swagger yapılandırması
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/api/applications", async (AppDbContext db) =>
{
   return Results.Ok(await db.JobApplications.ToListAsync());
})
.WithName("GetJobTracks");

app.MapGet("/api/applications/{id}", async (int id, AppDbContext db) =>
{
    var application = await db.JobApplications.FindAsync(id);
    if (application is null)
    {
        return Results.NotFound();
    }

    return Results.Ok(application);
});

app.MapPut("/api/applications/{id}", async (int id, JobApplication UpdatedApplication, AppDbContext db, JobApplicationValidator validator) =>
{
    var validationError = validator.Validate(UpdatedApplication);
    if (validationError != null)
    {
        return Results.BadRequest(validationError);
    }
    var application = await db.JobApplications.FindAsync(id);

    if(application is null)
    {
        return Results.NotFound();
    }

    application.CompanyName=UpdatedApplication.CompanyName;
    application.Position=UpdatedApplication.Position;
    application.Location=UpdatedApplication.Location;
    application.Status=UpdatedApplication.Status;
    application.Notes=UpdatedApplication.Notes;

    return Results.Ok(application);
});

app.MapDelete("/api/application/{id}", async (int id, AppDbContext db) =>
{
    var application = await db.JobApplications.FindAsync(id);

    if(application is null){
        return Results.NotFound();
    }
    
    db.JobApplications.Remove(application);
    await db.SaveChangesAsync();

    return Results.NoContent();
}).WithName("MapDelete");

app.MapPost("/api/applications", async (JobApplication application, AppDbContext db, JobApplicationValidator validator) =>
{
    var validationError = validator.Validate(application);
    if (validationError != null)
    {
        return Results.BadRequest(validationError);
    }

    db.JobApplications.Add(application);
    await db.SaveChangesAsync();

    

    return Results.Created(
        $"/api/applications/{application.Id}",
        application
    );
});



app.Run();

