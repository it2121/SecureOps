using System.ComponentModel.DataAnnotations;

namespace SecureOps.Models
{
    public class Role
    {

        [Key]
        public int RoleId { get; set; }

        [Required]
        [MaxLength(50)]
        public string RoleName { get; set; }

        // Many-to-many → navigation property
        public ICollection<User> Users { get; set; } = new List<User>();

    }
}
