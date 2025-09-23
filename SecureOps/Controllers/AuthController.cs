using Microsoft.AspNetCore.Mvc;
using SecureOps.Data;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly JwtService _jwt;

    public AuthController(AppDbContext db, JwtService jwt)
    {
        _db = db;
        _jwt = jwt;
    }

    [HttpGet("me")]
    public IActionResult Me()
    {
        var token = Request.Cookies["authToken"];
        if (string.IsNullOrEmpty(token)) return Unauthorized();

        var claims = _jwt.GetClaimsFromToken(token);
        return Ok(claims);
    }


    [HttpPost("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("authToken"); // deletes the auth cookie
        return Ok();
    }
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        var user = _db.Users.FirstOrDefault(u => u.Email == request.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return Unauthorized();

        var token = _jwt.GenerateToken(user);

        return Ok(new { token, fullName = user.FullName }); // <-- return fullName


        //  var cookieOptions = new CookieOptions
        //  {
        //      HttpOnly = false,
        //      Secure = false,        // for localhost testing
        //      SameSite = SameSiteMode.Lax,
        //      Expires = DateTime.UtcNow.AddHours(10),
        //      Path = "/"
        //  };

        //  // Store token
        //  Response.Cookies.Append("authToken", token, cookieOptions);

        //  // Store full name in a separate cookie (not HttpOnly, so JS/Blazor can read it)
        ///*  Response.Cookies.Append("fullName", user.FullName, new CookieOptions
        //  {
        //      HttpOnly = false,
        //      Secure = false,        // for localhost testing
        //      SameSite = SameSiteMode.Lax,
        //      Expires = DateTime.UtcNow.AddHours(10),
        //      Path = "/"
        //  });*/

        //  return Ok(new { token, fullName = user.FullName });
    }
}
    public class LoginRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}
