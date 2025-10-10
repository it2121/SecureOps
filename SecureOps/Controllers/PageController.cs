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
        public async Task<IActionResult> GetPage([FromQuery] int PageId)
        {
            var page = await _db.Pages
                .Include(p => p.Roles)
                .FirstOrDefaultAsync(p => p.PageId == PageId);

            if (page == null)
                return NotFound();

            var result = new
            {
                page.PageId,
                page.PageName,
                page.PageUrl,
                Roles = page.Roles.Select(r => new { r.RoleId, r.RoleName }).ToList()
            };

            return Ok(result);
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var pages = await _db.Pages
                .Include(p => p.Roles)
                .ToListAsync();

            var pageDtos = pages.Select(p => new PageDtoWithRoles
            {
                PageId = p.PageId,
                PageName = p.PageName,
                PageUrl = p.PageUrl,
                Roles = p.Roles.Select(r => new RoleDto
                {
                    RoleId = r.RoleId,
                    RoleName = r.RoleName
                }).ToList()
            }).ToList();

            return Ok(pageDtos);
        }
        [HttpDelete("{PageId}")]
        public async Task<IActionResult> DeletePage(int PageId)
        {
            try
            {
                var Page = await _db.Pages.FindAsync(PageId);

                if (Page == null)
                    return NotFound(new { message = $"Page with Id {PageId} not found." });

                _db.Pages.Remove(Page);
                await _db.SaveChangesAsync();


                return Ok(new { message = $"Page with Id {PageId} deleted successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


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
