using Microsoft.EntityFrameworkCore;
using SecureOps.Data;

namespace SecureOps.Services
{
    public interface IAccessService
    {
        Task<bool> HasAccessAsync(IEnumerable<string> userRoles, string pageUrl);
    }

    public class AccessService : IAccessService
    {
        private readonly AppDbContext _db;

        public AccessService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<bool> HasAccessAsync(IEnumerable<string> userRoles, string pageUrl)
        {
            if (userRoles == null || !userRoles.Any())
                return false;

            var allowedRoles = await _db.Pages
                .Where(p => p.PageUrl == pageUrl)
                .SelectMany(p => p.Roles.Select(r => r.RoleName))
                .ToListAsync();

            return userRoles.Any(role =>
                allowedRoles.Contains(role, StringComparer.OrdinalIgnoreCase));
        }
    }
}
