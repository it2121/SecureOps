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
    public class IncidentController : ControllerBase
    {
        private readonly AppDbContext _db;


        public IncidentController(AppDbContext db)
        {
            _db = db;
        }

        
        [HttpGet("GetIncident")]
        public async Task<Incident> GetIncident([FromQuery] int IncidentId)
        {
            Incident incident = await _db.Incidents
        .Include(e => e.Employee)
        .FirstOrDefaultAsync(e => e.Id == IncidentId);

            if (incident == null) return null;

            return  incident ; 
        }

        [HttpGet("GetIncidentsOfEmployee")]
        public async Task<IActionResult> GetIncidentsOfEmployee([FromQuery] int EmpID)
        {
            var incidents = await _db.Incidents
       .Where(i => i.EmployeeId == EmpID)
       .Select(i => new IncidentWithEmpDto
       {
           Id = i.Id,
           EmpId = i.EmployeeId,
           Title = i.Title,
           Description = i.Description,
           Status = i.Status,
           ReportedAt = i.ReportedAt
       })
       .ToListAsync();

/*

            var employee = await _db.Employees
    .Include(e => e.Incidents)
    .FirstOrDefaultAsync(e => e.Id == EmpID);

           var incidents = employee?.Incidents ?? new List<Incident>();

*/
               

            return Ok(incidents);
        }

        [HttpPost("RegisterIncident")]
        public async Task<IActionResult> RegisterEmployee([FromBody] IncidentWithEmpDto Dto)
        {
            try
            {

                var createdIncedent = new Incident
                {
                    Title = Dto.Title,

       Description = Dto.Description,

                    PhotoPath = Dto.PhotoPath,

                    Status = Dto.Status,

                    ReportedAt  = Dto.ReportedAt,
                    EmployeeId = Dto.EmpId


                };




                _db.Incidents.Add(createdIncedent);
                await _db.SaveChangesAsync();

                return Ok(createdIncedent);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}
