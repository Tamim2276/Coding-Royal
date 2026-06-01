using System.Net.Http.Headers;
using Blazored.LocalStorage;

namespace ClashOfCodes.Services;

public class JwtAuthHandler : DelegatingHandler
{
    private readonly ILocalStorageService _localStorage;

    public JwtAuthHandler(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        try
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }
        catch (InvalidOperationException)
        {
            // localStorage requires JS interop which is unavailable during
            // prerender. Swallow the exception and send without a token.
            // Login and register don't need a token anyway.
        }

        return await base.SendAsync(request, cancellationToken);
    }
}