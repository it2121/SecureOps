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
        // Temporary in-memory users (replace with DB later)


        // GET: api/users
        public UsersController(AppDbContext db)
        {
            _db = db;
          
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



    }

  
 
}
