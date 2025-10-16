using System.ComponentModel.DataAnnotations;

namespace SecureOps.Models
{
    public class Department
    {

       
            public int Id { get; set; }

            [Required, MaxLength(200)]
            public string Name { get; set; } = "Department Name";
            

            // Foreign Key → Employee who reported
            public int? ManagerId { get; set; }


            public Employee? Manager { get; set; }



        public ICollection<Employee>? Employees { get; set; }

    }
}
