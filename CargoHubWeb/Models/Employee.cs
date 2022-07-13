using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CargoHubWeb.Models
{
    public class Employee
    {
        [Key]
        [DisplayName("Employee Number")]
        public int Number { get; set; }
        [Required]
        public string Password { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
    }
}
