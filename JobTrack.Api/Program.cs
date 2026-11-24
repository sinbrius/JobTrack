using JobTrack.Api.Data;
using JobTrack.Api.Endpoints;
using JobTrack.Api.Services;
using JobTrack.Api.Validators;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Veritabanı Bağlantısı
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=jobtrack.db"));

// 2. Bağımlılık Enjeksiyonu (Dependency Injection) Kayıtları
builder.Services.AddScoped<JobApplicationValidator>();
builder.Services.AddScoped<IJobApplicationService, JobApplicationService>();

// 3. Hata Yönetimi ve Swagger Yapılandırması
builder.Services.AddProblemDetails();
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
});
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 4. Endpoint'leri Tek Satırda Uygulamaya Bağlama
app.MapJobApplicationEndpoints();

app.Run();