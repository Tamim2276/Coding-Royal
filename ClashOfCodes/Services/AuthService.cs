using Blazored.LocalStorage;
using ClashOfCodes.Shared.Models;
using System.Net.Http.Json;

namespace ClashOfCodes.Services;

public class AuthService
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;

    public AuthService(HttpClient httpClient, ILocalStorageService localStorage)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
    }

    public async Task<bool> LoginAsync(LoginModel model)
    {
        //send the login request to the Api
        // then receive the response
        var response = await _httpClient.PostAsJsonAsync("http://localhost:5045/api/auth/login", model);

        if (response.IsSuccessStatusCode)
        {
            //read the Jwt token of the response
            var result = await response.Content.ReadFromJsonAsync<TokenResponse>();

            // save the token into browser's local storage
            await _localStorage.SetItemAsync("authToken", result!.Token);
            return true;
        }
        return false;
    }

    public async Task<string> RegisterAsync(RegisterModel model)
    {
        var response = await _httpClient.PostAsJsonAsync("http://localhost:5045/api/auth/register", model);

        if (response.IsSuccessStatusCode)
        {
            return "Registration Successful";
        }
        return "Registration failed. Username may be taken.";
    }

    // A tiny helper class to read the JSON response from API {
    private class TokenResponse
    {
        public string Token { get; set; } = string.Empty;
    }
}