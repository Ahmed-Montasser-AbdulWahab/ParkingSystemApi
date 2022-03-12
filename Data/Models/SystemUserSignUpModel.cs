using System.ComponentModel.DataAnnotations;

namespace Parking_System_API.Data.Models
{
    public class SystemUserSignUpModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Role { get; set; }

    }
}
