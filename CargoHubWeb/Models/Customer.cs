using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CargoHubWeb.Models
{
    public class Customer
    {
        [Key]
        [DisplayName("Customer ID")]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Address { get; set; }
        public string Province { get; set; }
        public string Email { get; set; }
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }
    }
}
