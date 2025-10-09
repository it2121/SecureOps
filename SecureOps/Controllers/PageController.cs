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
    public class PageController : ControllerBase
    {
        private readonly AppDbContext _db;


        public PageController(AppDbContext db)
        {
            _db = db;
        }


        [HttpGet("GetPage")]
        public async Task<Page> GetPage([FromQuery] int PageId)
        {
            Page Page = await _db.Pages

        .FirstOrDefaultAsync(e => e.PageId == PageId);

            if (Page == null) return null;

            return Page;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var Page = await _db.Pages

       .ToListAsync();




            return Ok(Page);
        }

        //[HttpDelete("{PageId}")]
        //public async Task<IActionResult> DeletePage(int PageId)
        //{
        //    try
        //    {
        //        var Page = await _db.Pages.FindAsync(PageId);

        //        if (Page == null)
        //            return NotFound(new { message = $"Page with Id {PageId} not found." });

        //        _db.Pages.Remove(Page);
        //        await _db.SaveChangesAsync();


        //        return Ok(new { message = $"Page with Id {PageId} deleted successfully." });
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { message = ex.Message });
        //    }
        //}


        [HttpPost("UpsertPage")]
        public async Task<IActionResult> UpsertPage(PageDto dto)
        {
            var page = await _db.Pages
                .Include(p => p.Roles)
                .FirstOrDefaultAsync(p => p.PageId == dto.PageId);

            if (page == null)
            {
                page = new Page();
                _db.Pages.Add(page);
            }

            page.PageName = dto.PageName;
            page.PageUrl = dto.PageUrl;

            // Update roles
            page.Roles.Clear();
            var roles = await _db.Roles
                .Where(r => dto.SelectedRoleIds.Contains(r.RoleId))
                .ToListAsync();
            foreach (var role in roles)
            {
                page.Roles.Add(role);
            }

            await _db.SaveChangesAsync();
            return Ok();
        }
    }
}
