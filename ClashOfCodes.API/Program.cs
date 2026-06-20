using System.Text;
using System.Text.Json.Serialization;
using ClashOfCodes.API.Data;
using ClashOfCodes.API.Services;
using ClashOfCodes.Shared.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Json;

using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

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

// Configure JWT authentication (if needed for API endpoints)
// 1. Get the settings from appsettings.json
var jwtSettings = builder.Configuration.GetSection("JwtSettings");

builder.Services.AddHttpClient<JudgeService>(client =>
{
    client.BaseAddress = new Uri("https://emkc.org/api/v2/piston");
});

// 2. Configure JWT authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; // 401 Unauthorized if not authenticated
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters // check the token's signature, issuer, audience, and expiration
    {
        //the secret signature
        ValidateIssuerSigningKey = true, // Ensure the token's signature is valid
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"]!)), // Use the secret key from configuration to validate the token's signature

        //Who made this token
        ValidateIssuer = true,// Ensure the token was issued by a trusted issuer
        ValidIssuer = jwtSettings["Issuer"], // The expected issuer of the token, as defined in appsettings.json

        //Who is this token for
        ValidateAudience = true,// Ensure the token is intended for our API
        ValidAudience = jwtSettings["Audience"],// The expected audience of the token, as defined in appsettings.json

        // When the token expires
        ValidateLifetime = true,// Ensure the token has not expired
        ClockSkew = TimeSpan.Zero// Tokens expire exactly on time
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Enable CORS for API endpoints
app.UseCors("AllowBlazor");

// Enable authentication and authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// Map controllers for API endpoints
app.MapControllers();

// Migrate the database on startup
app.MigrateDatabase();

app.Run();
