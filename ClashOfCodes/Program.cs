using ClashOfCodes.Components;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using ClashOfCodes.Services;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddBlazoredLocalStorage();

builder.Services.AddAuthentication();
builder.Services.AddAuthorizationCore(); //authorization services for Blazor WebAssembly
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();// Register the CustomAuthStateProvider as the implementation for AuthenticationStateProvider
// without you having to inject it manually in every page
builder.Services.AddCascadingAuthenticationState();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// Add services to the container.
// builder.Services.AddTransient<JwtAuthHandler>();

// Configure HttpClient to use the JwtAuthHandler for adding JWT tokens to outgoing requests
builder.Services.AddHttpClient("ClashOfCodesAPI", client =>
{
    client.BaseAddress = new Uri("http://localhost:5045/");
});
// .AddHttpMessageHandler<JwtAuthHandler>();

// Add services to the container.
builder.Services.AddScoped<AuthService>();


// Add services to the container.
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("ClashOfCodesAPI"));// Register the CustomAuthStateProvider as the implementation for AuthenticationStateProvider
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