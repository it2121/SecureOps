using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureOps.Data;
using SecureOps.Models.Dto;
using SecureOps.Models;
using System.Text.Json;

namespace SecureOps.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SaftyTaskController : ControllerBase
    {
        private readonly AppDbContext _db;


        public SaftyTaskController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet("GetSafetyTask")]
        public async Task<SafetyTask> GetSafetyTask([FromQuery] int SafetyTaskId)
        {
            SafetyTask safetyTask = await _db.SafetyTasks

        .FirstOrDefaultAsync(e => e.Id == SafetyTaskId);

            if (safetyTask == null) return null;

            return safetyTask;
        }
        [HttpPost("UpdateAssignedEmployee")]

        public async Task<IActionResult> UpdateAssignedEmployee([FromForm] string safetyTaskJson)

        {
            try
            {
                if (string.IsNullOrWhiteSpace(safetyTaskJson))
                    return BadRequest(new { message = "Invalid request data." });

                var safetyTask = JsonSerializer.Deserialize<SafetyTask>(safetyTaskJson);
                if (safetyTask == null)
                    return BadRequest(new { message = "Failed to parse SafetyTask data." });

                var existingSafetyTask = await _db.SafetyTasks.FindAsync(safetyTask.Id);
                if (existingSafetyTask == null)
                    return NotFound(new { message = "Safety task not found." });

                // Update only the fields you want
                existingSafetyTask.EmployeeId = safetyTask.EmployeeId;

                // Optional: if you really only want to update the employee assignment,
                // then comment out the other property updates.
                // existingSafetyTask.Title = safetyTask.Title;
                // existingSafetyTask.Description = safetyTask.Description;
                // existingSafetyTask.DueDate = safetyTask.DueDate;
                // existingSafetyTask.Status = safetyTask.Status;

                _db.SafetyTasks.Update(existingSafetyTask);
                await _db.SaveChangesAsync();

                return Ok(existingSafetyTask);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("UpsertSaftyTask")]

        public async Task<IActionResult> UpsertSaftyTask([FromForm] string safetyTaskJson  )

        {
            try
            {
                SafetyTask _saftyTask = JsonSerializer.Deserialize<SafetyTask>(safetyTaskJson);

               
                var existingSaftyTask = await _db.SafetyTasks.FindAsync(_saftyTask.Id);


                if (existingSaftyTask is not null)
                {


  
        existingSaftyTask.Title = _saftyTask.Title;
                    existingSaftyTask.Description = _saftyTask.Description;
                    existingSaftyTask.DueDate = _saftyTask.DueDate;
                    existingSaftyTask.Status = _saftyTask.Status;
              
                    //existingSaftyTask.EmployeeId = _saftyTask.EmployeeId;

                    _db.SafetyTasks.Update(existingSaftyTask);
                    await _db.SaveChangesAsync();
                   return Ok(existingSaftyTask);
                }
                else
                {

                  
            
                    var newSafetyTask = new SafetyTask
                    {

                        Title = _saftyTask.Title,
                       Description = _saftyTask.Description,
                        DueDate = _saftyTask.DueDate,
                        Status = _saftyTask.Status
                    };

                    await _db.SafetyTasks.AddAsync(newSafetyTask);
                    await _db.SaveChangesAsync();


                  

                    return Ok(newSafetyTask);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }



        [HttpGet("GetSaftyTaskOfEmployee")]
        public async Task<IActionResult> GetSaftyTaskOfEmployee([FromQuery] int EmpID)
        {
            var SaftyTasks = await _db.SafetyTasks
  .Where(d => d.EmployeeId == EmpID).Include(d => d.Employee)
        .ToListAsync(); 



            return Ok(SaftyTasks);
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var safetyTasks = await _db.SafetyTasks
                .Include(s => s.Employee) // ✅ include employee navigation
                .Select(s => new
                {
                    s.Id,
                    s.Title,
                    s.Description,
                    s.DueDate,
                    s.Status,
                    s.EmployeeId,
                    Employee = s.Employee == null ? null : new
                    {
                        s.Employee.Id,
                        s.Employee.FullName,
                        s.Employee.JobTitle
                    }
                })
                .ToListAsync();

            return Ok(safetyTasks);
        }
    }
}
