namespace SecureOps.Models.Dto
{
    public class EmployeeDto
    {

        public int Id { get; set; }
        public string FullName { get; set; }
        public string JobTitle { get; set; }
        public int UserId { get; set; }
        public string Email { get; set; }




        // 🆕 Optional extra info for profile display
        public string? PhoneNumber { get; set; }
        public string? Department { get; set; }

        // 🖼 Avatar support — path relative to wwwroot (e.g. "Avatars/3/avatar.jpg")
        public string? AvatarPath { get; set; }

    }
}
