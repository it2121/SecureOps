using System.ComponentModel.DataAnnotations;

namespace SecureOps.Models
{
    public class User
    {


        [Key]  // Primary key
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Username { get; set; }
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }

        [Required]
        [MaxLength(255)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MaxLength(255)]
        public string PasswordHash { get; set; }  // store hashed password, not plain text!

        [MaxLength(50)]

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? LastLogin { get; set; }



        // Many-to-many → navigation property
        public ICollection<Role> Roles { get; set; } = new List<Role>();




    }
}
