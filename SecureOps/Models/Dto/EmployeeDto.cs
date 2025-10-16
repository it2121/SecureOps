namespace SecureOps.Models.Dto
{
    public class EmployeeDto
    {

        public int Id { get; set; }
        public string FullName { get; set; }
        public string JobTitle { get; set; }
        public int UserId { get; set; }
        public string Email { get; set; }
       



        public string? PhoneNumber { get; set; }
        public Department? Department { get; set; }

     

    }
}
