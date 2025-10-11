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

        [HttpPost("UpsertSaftyTask")]

        public async Task<IActionResult> UpsertSaftyTask([FromForm] string safetyTaskJson)

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
              
                    existingSaftyTask.EmployeeId = _saftyTask.EmployeeId;

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
                        Status = _saftyTask.Status,

                        EmployeeId = _saftyTask.EmployeeId
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
  .Where(d => d.EmployeeId == EmpID).Include(d => d.EmployeeId)
        .ToListAsync(); 



            return Ok(SaftyTasks);
        }


        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var SDocuments = await _db.SafetyTasks

                .ToListAsync();

            return Ok(SDocuments);
        }
    }
}
