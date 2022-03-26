using System.ComponentModel.DataAnnotations;

namespace Parking_System_API.Data.Entities
{
    public class SystemUser //Admin operator
    {
        [Required, Key]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public string Salt { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public bool IsAdmin { get; set; }

        [Required]
        public bool IsPowerAccount { get; set; }
    }
}
