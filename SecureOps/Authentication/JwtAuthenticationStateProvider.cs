using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Blazored.LocalStorage;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
public class JwtAuthenticationStateProvider : AuthenticationStateProvider
{
    private string _token;
    private readonly ILocalStorageService _localStorage;
    private const string TokenKey = "authToken";

    public JwtAuthenticationStateProvider(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }
    public async Task SetTokenAsync(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            await _localStorage.RemoveItemAsync(TokenKey);
        }
        else
        {
            await _localStorage.SetItemAsync(TokenKey, token);
        }
      //  _token = token;
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
    public void SetToken(string token)
    {
        _token = token;
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await _localStorage.GetItemAsync<string>(TokenKey);

        if (string.IsNullOrWhiteSpace(token))
        {
            // no token → anonymous user
            var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
            return new AuthenticationState(anonymous);
        }

        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);

        var identity = new ClaimsIdentity(jwt.Claims, "jwt");
        var user = new ClaimsPrincipal(identity);

        return new AuthenticationState(user);
    }

}
