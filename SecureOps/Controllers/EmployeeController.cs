using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureOps.Data;
using SecureOps.Models;
using SecureOps.Models.Dto;
using System.Text.Json;

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


        [HttpPost("SetAvatar")]
        public async Task<IActionResult> SetAvatar([FromForm] string EmpDtoWithIdJson, [FromForm] IFormFile file)
        {
            try
            {
                var dto = JsonSerializer.Deserialize<EmployeeDto>(EmpDtoWithIdJson);

                if (dto == null || file == null)
                    return BadRequest("Invalid employee data or file.");

                var existingEmp = await _db.Employees.FindAsync(dto.Id);
                if (existingEmp is null)
                    return NotFound("Employee not found.");

                // Define folder path for this employee
                string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Avatars", dto.Id.ToString());

                // Create folder if it doesn’t exist
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                // Delete all existing files inside the folder
                foreach (var oldFile in Directory.GetFiles(folderPath))
                {
                    System.IO.File.Delete(oldFile);
                }

                // Get file extension (.jpg, .png, etc.)
                string fileExtension = Path.GetExtension(file.FileName);

                // Create new file name
                string fileName = $"avatar-{dto.Id}{fileExtension}";
                string filePath = Path.Combine(folderPath, fileName);

                // Save the new file asynchronously
                await using (var fs = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fs);
                }

            

                return Ok(new { message = "Avatar updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error setting avatar", details = ex.Message });
            }
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

        [HttpPost("UpsertEmployee")]
        public async Task<IActionResult> UpsertEmployee([FromForm] string employeeDtoJson)
        {
            if (string.IsNullOrWhiteSpace(employeeDtoJson))
                return BadRequest("Missing employee data.");

            EmployeeDto? employeeDto;

            try
            {
                employeeDto = JsonSerializer.Deserialize<EmployeeDto>(
                    employeeDtoJson,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );
            }
            catch (Exception ex)
            {
                return BadRequest($"Invalid JSON format: {ex.Message}");
            }

            if (employeeDto == null)
                return BadRequest("Invalid employee object.");

            // Look for existing employee
            var existingEmployee = await _db.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.Id == employeeDto.Id);

            if (existingEmployee != null)
            {
                // Update existing record
                existingEmployee.FullName = employeeDto.FullName;
                existingEmployee.Email = employeeDto.Email;
                existingEmployee.PhoneNumber = employeeDto.PhoneNumber;

            

                _db.Employees.Update(existingEmployee);
            }
            else
            {
                // Insert new record
                var newEmployee = new Employee
                {
                    FullName = employeeDto.FullName,
                    Email = employeeDto.Email,
                    PhoneNumber = employeeDto.PhoneNumber,
                };

                _db.Employees.Add(newEmployee);
            }

            await _db.SaveChangesAsync();
            return Ok(new { success = true });
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
