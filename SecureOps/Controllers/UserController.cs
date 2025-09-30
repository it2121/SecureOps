using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureOps.Data;
using SecureOps.Models;
using SecureOps.Models.Dto;


namespace SecureOps.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
    private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly PasswordHasher<User> _passwordHasher;

        // Temporary in-memory users (replace with DB later)


        // GET: api/users

        public UsersController(AppDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
            _passwordHasher = new PasswordHasher<User>();

        }


        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var users = await _db.Users
                .Select(u => new UserDto
                {
                    Email = u.Email,
                    FullName = u.FullName,
                    Username = u.Username
                })
                .ToListAsync();

            return Ok(users);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("GetNameFromEmail")]
        public async Task<IActionResult> GetNameFromEmail([FromQuery] string email)
        {
            if (string.IsNullOrEmpty(email))
                return BadRequest("Email is required.");

            var user = await _db.Users
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
                return NotFound("User not found.");

            var dto = new UserDto
            {
                Email = user.Email,
                FullName = user.FullName,
                Username = user.Username
            };

            return Ok(dto);
        }


        [HttpPost("RegisterUser")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterDto user)
        {
            try
            {
              

           
            // Check if username or email already exists
            if (await _db.Users.AnyAsync(u => u.Username == user.Username || u.Email == user.Email))
            {
                throw new InvalidOperationException("Username or Email already exists.");
            }

            var createdUser = new User
            {
                Username = user.Username,
                FullName = user.FullName,
                Email = user.Email,
                CreatedAt = DateTime.UtcNow
            };

                // Hash the password before saving
                createdUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);

                // Add default role (optional)
                var defaultRole = await _db.Roles.FirstOrDefaultAsync(r => r.RoleName == "Viewer");
            if (defaultRole != null)
            {
                    createdUser.Roles.Add(defaultRole);
            }

            _db.Users.Add(createdUser);
            await _db.SaveChangesAsync();

            return Ok(createdUser);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }

  
 
}
