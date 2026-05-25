using System.Text.Json.Serialization;
using ClashOfCodes.API.Data;
using ClashOfCodes.Shared.Models;
using Microsoft.AspNetCore.Http.Json;

using Microsoft.AspNetCore.Identity;

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

// Configure Identity with custom password options and EF Core stores
builder.Services.AddIdentity<User, Microsoft.AspNetCore.Identity.IdentityRole<int>>(option =>
{

    option.Password.RequireDigit = false;
    option.Password.RequiredLength = 6;
    option.Password.RequireNonAlphanumeric = false;
    option.Password.RequireUppercase = false;
    option.Password.RequireLowercase = false;
})
.AddEntityFrameworkStores<AppDbContext>() // Use our AppDbContext for Identity stores
.AddDefaultTokenProviders();// Add default token providers for password reset, email confirmation, etc.

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
