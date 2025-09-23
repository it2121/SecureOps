using SecureOps.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

public static class JwtHelper
{
    public static User DecodeTokenToUser(string token)
    {
        if (string.IsNullOrEmpty(token)) return null;

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        var user = new User
        {
            Email = jwtToken.Claims.FirstOrDefault(c => c.Type == "Email")?.Value
                    ?? jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value,
            FullName = jwtToken.Claims.FirstOrDefault(c => c.Type == "FullName")?.Value,
            Username = jwtToken.Claims.FirstOrDefault(c => c.Type == "Username")?.Value,

            Roles = jwtToken.Claims
                            .Where(c => c.Type == ClaimTypes.Role || c.Type == "role")
                            .Select(c => new Role { RoleName = c.Value })
                            .ToList()
        };

        return user;
    }
}
