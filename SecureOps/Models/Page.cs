using System.ComponentModel.DataAnnotations;

namespace SecureOps.Models
{
    public class Page
    {
        [Key]
        public int PageId { get; set; }

        [Required]
        [MaxLength(100)]
        public string PageName { get; set; }

        [Required]
        [MaxLength(200)]
        public string PageUrl { get; set; }

        // Many-to-many → navigation property
        public ICollection<Role> Roles { get; set; } = new List<Role>();
    }
}
