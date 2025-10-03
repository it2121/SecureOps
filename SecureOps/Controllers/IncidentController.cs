using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureOps.Data;
using SecureOps.Models;
using SecureOps.Models.Dto;
using System.Globalization;

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






        public async Task<string> MoveAndRenamePhotoAsync(string originalFilePath)
        {
            if (string.IsNullOrEmpty(originalFilePath) || !System.IO.File.Exists(originalFilePath))
                return null;

            // 1. Ensure photos folder exists
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/photos");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            // 2. Create unique filename with date and time to the second
            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss", CultureInfo.InvariantCulture);
            var extension = Path.GetExtension(originalFilePath);
            var newFileName = $"{timestamp}_{Guid.NewGuid().ToString().Substring(0, 8)}{extension}";

            var newFilePath = Path.Combine(folderPath, newFileName);

            // 3. Move the file
            System.IO.File.Copy(originalFilePath, newFilePath); // copy first, you can use Move if you want
                                                                // System.IO.File.Delete(originalFilePath); // uncomment if you want to remove the original

            // 4. Return relative path to store in DB
            var relativePath = $"photos/{newFileName}";
            return relativePath;
        }







        //public async Task<string> SavePhotoAsync(IFormFile photo)
        //{
        //    if (photo == null || photo.Length == 0)
        //        return null;

        //    // 1. Get folder path
        //    var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/photos");
        //    if (!Directory.Exists(folderPath))
        //        Directory.CreateDirectory(folderPath);

        //    // 2. Create unique filename with date and time to second
        //    var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss", CultureInfo.InvariantCulture);
        //    var extension = Path.GetExtension(photo.FileName); // keep original extension
        //    var fileName = $"{timestamp}_{Guid.NewGuid().ToString().Substring(0, 8)}{extension}";

        //    var filePath = Path.Combine(folderPath, fileName);

        //    // 3. Save the file
        //    using (var stream = new FileStream(filePath, FileMode.Create))
        //    {
        //        await photo.CopyToAsync(stream);
        //    }

        //    // 4. Return relative path to store in DB
        //    var relativePath = $"photos/{fileName}";
        //    return relativePath;
        //}



        [HttpGet("GetIncident")]
        public async Task<Incident> GetIncident([FromQuery] int IncidentId)
        {
            Incident incident = await _db.Incidents
      
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
        public async Task<IActionResult> RegisterIncident([FromBody] IncidentWithEmpDto Dto)
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

        private string? MoveAndRenamePhoto(string originalFilePath)
        {
            if (string.IsNullOrEmpty(originalFilePath) || !System.IO.File.Exists(originalFilePath))
                return null;

            var folderPath = Path.GetDirectoryName(originalFilePath);
            if (string.IsNullOrEmpty(folderPath))
                return null;

            // Generate new name with timestamp + GUID
            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss", CultureInfo.InvariantCulture);
            var extension = Path.GetExtension(originalFilePath);
            var newFileName = $"{timestamp}_{Guid.NewGuid().ToString().Substring(0, 8)}{extension}";
            var newFilePath = Path.Combine(folderPath, newFileName);

            // Rename the file
            System.IO.File.Move(originalFilePath, newFilePath);

            // Return just the new file name
           // Console.WriteLine("original "+originalFilePath + " new"+ newFileName);
            return newFilePath;
        }


        [HttpPost("UpsertIncident")]
        public async Task<IActionResult> UpsertIncident([FromBody] IncidentWithEmpDto dto)
        {
            try
            {
                // Move & rename the photo if provided
                string? savedPhotoPath = null;
                if (!string.IsNullOrEmpty(dto.PhotoPath))
                {
                    savedPhotoPath = MoveAndRenamePhoto(dto.PhotoPath);
                }


                // Check if incident exists (by Id in the DTO)
                var existingIncident = await _db.Incidents.FindAsync(dto.Id);

                if (existingIncident is not null)
                {
                    // Update existing
                    existingIncident.Title = dto.Title;
                    existingIncident.Description = dto.Description;
                    existingIncident.PhotoPath = savedPhotoPath ?? existingIncident.PhotoPath;
                    existingIncident.Status = dto.Status;
                    existingIncident.ReportedAt = dto.ReportedAt;
                    existingIncident.EmployeeId = dto.EmpId;

                    _db.Incidents.Update(existingIncident);
                    await _db.SaveChangesAsync();

                    return Ok(existingIncident);
                }
                else
                {
                    // Create new
                    var newIncident = new Incident
                    {
                        Title = dto.Title,
                        Description = dto.Description,
                        PhotoPath = savedPhotoPath,
                        Status = dto.Status,
                        ReportedAt = dto.ReportedAt,
                        EmployeeId = dto.EmpId
                    };

                    await _db.Incidents.AddAsync(newIncident);
                    await _db.SaveChangesAsync();

                    return Ok(newIncident);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIncident(int id)
        {
            try
            {
                var incident = await _db.Incidents.FindAsync(id);

                if (incident == null)
                    return NotFound(new { message = $"Incident with Id {id} not found." });

                _db.Incidents.Remove(incident);
                await _db.SaveChangesAsync();

                return Ok(new { message = $"Incident with Id {id} deleted successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
