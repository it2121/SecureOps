using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureOps.Data;
using SecureOps.Models.Dto;

namespace SecureOps.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SDocumentController : ControllerBase
    {
        private readonly AppDbContext _db;


        public SDocumentController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet("GetSDocumentOfEmployee")]
        public async Task<IActionResult> GetSDocumentOfEmployee([FromQuery] int EmpID)
        {
            var SDocuments = await _db.SDocuments
  .Where(d => d.UploadedById == EmpID).Include(d => d.UploadedBy)
        .ToListAsync(); // execute query




            return Ok(SDocuments);
        }


        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var SDocuments = await _db.SDocuments

                .ToListAsync();

            return Ok(SDocuments);
        }

    }
}
