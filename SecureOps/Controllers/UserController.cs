using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureOps.Data;
using SecureOps.Models.Dto;


namespace SecureOps.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
    private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        // Temporary in-memory users (replace with DB later)


        // GET: api/users
   
        public UsersController(AppDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
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

    }

  
 
}
