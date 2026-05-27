using ClashOfCodes.Components;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using ClashOfCodes.Services;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddBlazoredLocalStorage();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// Add services to the container.
builder.Services.AddScoped(sp => new HttpClient());

// Add services to the container.
builder.Services.AddScoped<ClashOfCodes.Services.AuthService>();

// Add services to the container.
builder.Services.AddAuthorizationCore();

// Register the CustomAuthStateProvider as the implementation for AuthenticationStateProvider
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
