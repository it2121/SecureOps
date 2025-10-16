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
                PhoneNumber = employee.PhoneNumber,
                FullName = employee.FullName,
                JobTitle = employee.JobTitle,
                UserId = employee.User?.Id ?? 0,
                Email   = employee.Email,
                Department = employee.Department

            };
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var employees = await _db.Employees
                
                .ToListAsync();

            return Ok(employees);
        }

        [HttpPost("SetDepartment")]
        public async Task<IActionResult> SetDepartment([FromBody] int employeeId, int departmentId)
        {
            if (employeeId <= 0 || departmentId <= 0)
                return BadRequest(new { message = "Invalid employee or department ID." });

            var employee = await _db.Employees.FirstOrDefaultAsync(e => e.Id == employeeId);
            if (employee == null)
                return NotFound(new { message = "Employee not found." });

            var department = await _db.Departments.FirstOrDefaultAsync(d => d.Id == departmentId);
            if (department == null)
                return NotFound(new { message = "Department not found." });

            // Assign or update department
            employee.DepartmentId = departmentId;

            _db.Employees.Update(employee);
            await _db.SaveChangesAsync();

            return Ok(new
            {
                message = "Department assignment updated successfully.",
                employee = new
                {
                    employee.Id,
                    employee.FullName,
                    employee.JobTitle,
                    employee.Email,
                    Department = department.Name
                }
            });
        }


        [HttpPost("RegisterEmployee")]
        public async Task<IActionResult> RegisterEmployee([FromBody] EmployeeDto Emp)
        {
            try
            {

                var createdEmp = new Employee
                {
                    FullName = Emp.FullName,
                    Email = Emp.Email,
                    PhoneNumber = Emp.PhoneNumber,
                    UserId = Emp.UserId,
                    JobTitle = Emp.JobTitle,
                };

              
               

                _db.Employees.Add(createdEmp);
                await _db.SaveChangesAsync();

                return Ok(createdEmp);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }



    }

}
