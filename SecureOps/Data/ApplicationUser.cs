using Microsoft.AspNetCore.Identity;

namespace SecureOps.Data
{
 
        public class ApplicationUser : IdentityUser
        {
            public string FullName { get; set; }
        }
    
}
