using System.ComponentModel.DataAnnotations;

namespace CustomerManagement.Models
{
    public class Customer
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public int Age { get; set; }

        [Required]
        public int Id { get; set; }
    }
}
