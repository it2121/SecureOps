using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureOps.Data;
using SecureOps.Models.Dto;

namespace SecureOps.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentController : ControllerBase
    {
        private readonly AppDbContext _db;


        public DepartmentController(AppDbContext db)
        {
            _db = db;
        }


        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var Departments = await _db.Departments

                .ToListAsync();

            return Ok(Departments);
        }

        [HttpGet("GetAllWithEmployees")]
        public async Task<IActionResult> GetAllWithEmployees()
        {
            var departments = await _db.Departments
                .Include(d => d.Employees)      // Include all employees in the department
                .Include(d => d.Manager)        // Include the manager as Employee
                .Select(d => new
                {
                    d.Id,
                    d.Name,
                    Manager = d.Manager != null ? new
                    {
                        d.Manager.Id,
                        d.Manager.FullName,
                        d.Manager.Email,
                        d.Manager.JobTitle,
                        d.Manager.PhoneNumber
                    } : null,
                    Employees = d.Employees.Select(e => new
                    {
                        e.Id,
                        e.FullName,
                        e.Email,
                        e.JobTitle,
                        e.PhoneNumber
                    }).ToList()
                })
                .ToListAsync();

            return Ok(departments);
        }

        [HttpPost("Upsert")]
        public async Task<IActionResult> Upsert([FromBody] DepartmentDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return BadRequest(new { message = "Department name is required." });

            try
            {
                // Try to find existing department
                var department = await _db.Departments
                    .FirstOrDefaultAsync(d => d.Id == request.Id);

                if (department == null)
                {
                    // Create new
                    department = new SecureOps.Models.Department
                    {
                        Name = request.Name,
                        ManagerId = request.ManagerId
                    };
                    _db.Departments.Add(department);
                }
                else
                {
                    // Update existing
                    department.Name = request.Name;
                    department.ManagerId = request.ManagerId;
                    _db.Departments.Update(department);
                }

                await _db.SaveChangesAsync();

                return Ok(new
                {
                    message = department.Id == request.Id
                        ? "Department updated successfully."
                        : "Department created successfully.",
                    department
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
