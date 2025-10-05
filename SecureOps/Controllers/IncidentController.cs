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
    public class IncidentController : ControllerBase
    {
        private readonly AppDbContext _db;


        public IncidentController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet("GetCurrentIncidentID")]
        public async Task<ActionResult<int>> GetCurrentIncidentID()
        {
            

            var nextId = await _db.Incidents
.FromSqlRaw("SELECT CAST(IDENT_CURRENT('Incidents') + IDENT_INCR('Incidents') AS int) AS Id")
.Select(x => x.Id)
.FirstAsync(); return Ok(nextId-1);
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
                                                               

            // 4. Return relative path to store in DB
            var relativePath = $"photos/{newFileName}";
            return relativePath;
        }








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

        public async Task<IActionResult> UpsertIncident([FromForm] string dtoJson, [FromForm] List<IFormFile> files)

        {
            try
            {
                IncidentWithEmpDto dto = JsonSerializer.Deserialize<IncidentWithEmpDto>(dtoJson);

                // Move & rename the photo if provided
           


                // Check if incident exists (by Id in the DTO)
                var existingIncident = await _db.Incidents.FindAsync(dto.Id);


                if (existingIncident is not null)
                {

                    string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\photos\\" + dto.Id);
                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);
                    dto.PhotoPath = folderPath;
                    // Update existing
                    existingIncident.Title = dto.Title;
                    existingIncident.Description = dto.Description;
                    existingIncident.PhotoPath = dto.PhotoPath;
                    existingIncident.Status = dto.Status;
                    existingIncident.ReportedAt = dto.ReportedAt;
                    existingIncident.EmployeeId = dto.EmpId;

                    _db.Incidents.Update(existingIncident);
                    await _db.SaveChangesAsync();




                    foreach (var file in files)
                    {
                      
                       

                        // Create unique filename
                        var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                        var extension = Path.GetExtension(file.FileName);
                        var fileName = Path.GetFileName(file.FileName)  + "_" + Guid.NewGuid().ToString().Substring(0, 8) + extension;
                        var filePath = Path.Combine(dto.PhotoPath, fileName);
                        // Save file to server
                

                        await using var stream = file.OpenReadStream(); // 100 MB
                        await using var fs = new FileStream(filePath, FileMode.Create);
                        await stream.CopyToAsync(fs);

                    }


                    return Ok(existingIncident);
                }
                else
                {

                    //    var lastInsideant = await _db.Incidents
                    //.OrderByDescending(e => e.Id)
                    //.FirstOrDefaultAsync();

                    // int nextId = (lastInsideant != null) ? lastInsideant.Id + 1 : 1;
                    //var lastId = await _db.Incidents.MaxAsync(e => (int?)e.Id) ?? 0;
                    //var nextId = lastId + 1;
                    var nextId = await _db.Incidents
      .FromSqlRaw("SELECT CAST(IDENT_CURRENT('Incidents') + IDENT_INCR('Incidents') AS int) AS Id")
      .Select(x => x.Id)
      .FirstAsync();

                    // Create new
                    string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\photos\\" + nextId);
                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);
                    dto.PhotoPath = folderPath;
                   var newIncident = new Incident
                    {
                        Title = dto.Title,
                        Description = dto.Description,
                        PhotoPath = dto.PhotoPath,
                        Status = dto.Status,
                        ReportedAt = dto.ReportedAt,
                        EmployeeId = dto.EmpId
                    };

                    await _db.Incidents.AddAsync(newIncident);
                    await _db.SaveChangesAsync();

                    
                    foreach (var file in files)
                    {


                     

                        // Create unique filename
                        var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                        var extension = Path.GetExtension(file.FileName);
                        var fileName = Path.GetFileName(file.FileName) + "_" + Guid.NewGuid().ToString().Substring(0, 8) + extension;
                        var filePath = Path.Combine(folderPath, fileName);
                        // Save file to server
                        //using var stream = file.OpenReadStream(); // 60MB max
                        //using var fs = new FileStream(filePath, FileMode.Create);
                        //await stream.CopyToAsync(fs);

                       /* await using var stream = new FileStream(filePath, FileMode.Create);

                        await file.CopyToAsync(stream);*/

                        await using var stream = file.OpenReadStream(); // 100 MB
                        await using var fs = new FileStream(filePath, FileMode.Create);

                        await stream.CopyToAsync(fs);
                    }

                    return Ok(newIncident);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("DeleteImage")]
        public IActionResult DeleteImage([FromQuery] string relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
                return BadRequest("Invalid image path");

            try
            {
                // Combine wwwroot with relative path
                var rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                var fullPath = Path.Combine(rootPath, relativePath.TrimStart('\\', '/'));

                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                    return Ok(new { message = "Image deleted successfully" });
                }
                else
                {
                    return NotFound(new { message = "Image not found" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error deleting image", error = ex.Message });
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

                string folderPath = Path.Combine("wwwroot\\photos\\" + id);

                if (Directory.Exists(folderPath))
                {
                    Directory.Delete(folderPath, recursive: true);

                }

                return Ok(new { message = $"Incident with Id {id} deleted successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
