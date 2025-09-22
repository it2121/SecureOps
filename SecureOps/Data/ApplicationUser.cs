using Microsoft.AspNetCore.Identity;

namespace SecureOps.Data
{
 
        public class ApplicationUser : IdentityUser
        {
            public string FullName { get; set; }
            public string Email { get; set; }
            public string UserName { get; set; }



        }
    
}
