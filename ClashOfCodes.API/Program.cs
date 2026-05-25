using System.Text.Json.Serialization;
using ClashOfCodes.API.Data;
using Microsoft.AspNetCore.Http.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add services to the container.
// Configure JSON options to handle reference loops in entity relationships
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

// Configure CORS to allow requests from the Blazor client application
// Learn more about configuring CORS at https://aka.ms/aspnetcore/cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazor", policy =>
    {
        policy.WithOrigins("https://localhost:7125", "http://localhost:5212")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Remove Http.Json options config, as AddJsonOptions above configures it for Controllers

builder.AddClashOfCodesDbContext();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Enable CORS for API endpoints
app.UseCors("AllowBlazor");

// Map controllers for API endpoints
app.MapControllers();

// Migrate the database on startup
app.MigrateDatabase();

app.Run();
