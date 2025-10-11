using Microsoft.IdentityModel.Tokens;
using SecureOps.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class JwtService
{
    private readonly IConfiguration _config;
    public JwtService(IConfiguration config) => _config = config;
    public ClaimsPrincipal GetClaimsFromToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);
        var identity = new ClaimsIdentity(jwt.Claims, "jwt");
        return new ClaimsPrincipal(identity);
    }
    public string GenerateToken(User user, List<string> roles,Employee employee)
    { // Build all claims including roles
        var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim("Email", user.Email),
        new Claim("UserId", user.Id.ToString()),
        new Claim("FullName", user.FullName),
        new Claim("Username", user.Username),
        new Claim("PasswordHash", user.PasswordHash),
   



    };

        if (employee != null)
        {
            if (employee.Id != 0)
                claims.Add(new Claim("employeeId", employee.Id.ToString()));

            if (employee.UserId != 0)
                claims.Add(new Claim("employeeUserId", employee.UserId.ToString()));
            if (employee.UserId != 0)
                claims.Add(new Claim("employeeJobTitle", employee.JobTitle.ToString()));
            if (employee.UserId != 0)
                claims.Add(new Claim("employeeFullName", employee.FullName.ToString()));


        }


        // Add all roless
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var key = Encoding.ASCII.GetBytes(_config["JwtSettings:SecretKey"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),  // <--- use the list including roles
            Expires = DateTime.UtcNow.AddMinutes(double.Parse(_config["JwtSettings:ExpiryMinutes"])),
            Issuer = _config["JwtSettings:Issuer"],
            Audience = _config["JwtSettings:Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);

    }
}
