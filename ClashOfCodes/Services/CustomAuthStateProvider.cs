using System.Security.Claims;
using System.Text.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace ClashOfCodes.Services;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorage;

    public CustomAuthStateProvider(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var savedToken = await _localStorage.GetItemAsync<string>("authToken");

        if (string.IsNullOrWhiteSpace(savedToken))
        {
            //No token found. the user is a nobody
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        //the user has a token lets decode it
        var claims = ParseClaimsFromJwt(savedToken);
        var identity = new ClaimsIdentity(claims, "jwt");
        var user = new ClaimsPrincipal(identity);

        return new AuthenticationState(user);
    }

    private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var payload = jwt.Split('.')[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
        return keyValuePairs!.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()!));
    }

    // Helper to fix Base64 formatting from the JWT standard
    private byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }
        return Convert.FromBase64String(base64);
    }

    public void MarkUserAsAuthenticated(string token)
    {
        var claims = ParseClaimsFromJwt(token);
        var identity = new ClaimsIdentity(claims, "jwt");
        var authenticatedUser = new ClaimsPrincipal(identity);
        var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
        // Shouts through a megaphone to the whole app: "Hey UI, re-render! The user just logged in!"
        NotifyAuthenticationStateChanged(authState);
    }

    public void MarkUserAsLoggedOut()
    {
        var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
        var authState = Task.FromResult(new AuthenticationState(anonymousUser));
        // Shouts through a megaphone to the whole app: "Hey UI, re-render! The user logged out!"
        NotifyAuthenticationStateChanged(authState);
    }
}