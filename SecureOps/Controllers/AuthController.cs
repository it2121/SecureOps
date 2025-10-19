using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        var user = _db.Users
     .Include(u => u.Roles)  // <-- load roles
         .Include(u => u.Employee).Include(u => u.Company) // <-- important

     .FirstOrDefault(u => u.Email == request.Email);



        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return Unauthorized();
        var roles = user.Roles.Select(r => r.RoleName).ToList();
        var employee = user.Employee;
        var company = user.Company;

        var token = _jwt.GenerateToken(user, roles, employee, company);

        return Ok(new { token }); // <-- return fullName


    }
}
    public class LoginRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}
