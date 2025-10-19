namespace SecureOps.Models.Dto
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public ICollection<Role> Roles { get; set; } = new List<Role>();
        public Company? Company { get; set; }
        public int? CompanyId { get; set; }

    }
}
