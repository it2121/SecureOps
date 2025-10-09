using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureOps.Data;
using SecureOps.Models;
using SecureOps.Models.Dto;
using SecureOps.Pages;
using System.Globalization;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace SecureOps.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly AppDbContext _db;


        public RoleController(AppDbContext db)
        {
            _db = db;
        }


        [HttpGet("GetRole")]
        public async Task<Role> GetRole([FromQuery] int roleId)
        {
            Role role = await _db.Roles

        .FirstOrDefaultAsync(e => e.RoleId == roleId);

            if (role == null) return null;

            return role;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var role = await _db.Roles
    
       .ToListAsync();




            return Ok(role);
        }
        [HttpDelete("{roleId}")]
        public async Task<IActionResult> DeleteRole(int roleId)
        {
            try
            {
                var role = await _db.Roles.FindAsync(roleId);

                if (role == null)
                    return NotFound(new { message = $"Role with Id {roleId} not found." });

                _db.Roles.Remove(role);
                await _db.SaveChangesAsync();

                
                return Ok(new { message = $"Role with Id {roleId} deleted successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpPost("UpsertRole")]

        public async Task<IActionResult> UpsertRole([FromForm] string roleJson)

        {
            try
            {
                Role role = JsonSerializer.Deserialize<Role>(roleJson);

                // Move & rename the photo if provided



                // Check if incident exists (by Id in the DTO)
                var existingRole = await _db.Roles.FindAsync(role.RoleId);


                if (existingRole is not null)
                {

                existingRole.RoleName = role.RoleName;  
                    

                    _db.Roles.Update(existingRole);
                    await _db.SaveChangesAsync();




                   


                    return Ok(existingRole);
                }
                else
                {

                  
                    

                    var newRole = new Role
                    {
                        RoleName = role.RoleName
                    };
                   

                    await _db.Roles.AddAsync(newRole);
                    await _db.SaveChangesAsync();


                  

                    return Ok(newRole);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}
