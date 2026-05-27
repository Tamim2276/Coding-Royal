using Blazored.LocalStorage;
using ClashOfCodes.Shared.Models;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;

namespace ClashOfCodes.Services;

public class AuthService
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;
    private readonly AuthenticationStateProvider _authStateProvider;
    public AuthService(HttpClient httpClient, ILocalStorageService localStorage, AuthenticationStateProvider authStateProvider)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
        // Set the base address for the HttpClient to point to the API server
        _httpClient.BaseAddress = new Uri("http://localhost:5045/");
        _authStateProvider = authStateProvider;
    }

    public async Task<string> LoginAsync(LoginModel model)
    {
        //send the login request to the Api
        // then receive the response
        var response = await _httpClient.PostAsJsonAsync("api/auth/login", model);

        if (response.IsSuccessStatusCode)
        {
            //read the Jwt token of the response
            var result = await response.Content.ReadFromJsonAsync<TokenResponse>();

            // save the token into browser's local storage
            await _localStorage.SetItemAsync("authToken", result!.Token);
            // tell the app that the user is authenticated now
            if (_authStateProvider is CustomAuthStateProvider customAuthStateProvider)
            {
                customAuthStateProvider.MarkUserAsAuthenticated(result.Token);
            }
            return "Login Successful";
        }
        var error = await response.Content.ReadAsStringAsync();
        return $"Login failed. {error}";
    }

    public async Task<string> RegisterAsync(RegisterModel model)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/register", model);

        if (response.IsSuccessStatusCode)
        {
            return "Registration Successful";
        }
        var error = await response.Content.ReadAsStringAsync();
        return $"Registration failed. {error}";
    }

    public async Task LogoutAsync()
    {
        // remove the token from local storage
        await _localStorage.RemoveItemAsync("authToken");
        // tell the app that the user is logged out now
        if (_authStateProvider is CustomAuthStateProvider customAuthStateProvider)
        {
            customAuthStateProvider.MarkUserAsLoggedOut();
        }
    }

    // A tiny helper class to read the JSON response from API {
    private class TokenResponse
    {
        public string Token { get; set; } = string.Empty;
    }
}