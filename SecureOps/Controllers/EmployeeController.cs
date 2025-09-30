using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureOps.Data;
using SecureOps.Models;
using SecureOps.Models.Dto;

namespace SecureOps.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly AppDbContext _db;


        public EmployeeController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet("GetEmployee")]
        public async Task<EmployeeDto> GetEmployee([FromQuery] int empId)
        {
            var employee = await _db.Employees
        .Include(e => e.User)
        .FirstOrDefaultAsync(e => e.Id == empId);

            if (employee == null) return null;

            return new EmployeeDto
            {
                Id = employee.Id,
                FullName = employee.FullName,
                JobTitle = employee.JobTitle,
                UserId = employee.User?.Id ?? 0,
                Username = employee.User?.Username,
                Email   = employee.Email,
                
            };
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var employees = await _db.Employees
                
                .ToListAsync();

            return Ok(employees);
        }
    }

}
