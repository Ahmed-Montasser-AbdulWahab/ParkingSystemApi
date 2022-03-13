using System.ComponentModel.DataAnnotations;

namespace Parking_System_API.Data.Models
{
    public class ForgetPasswordModel
    {
        [Required]
        public string Email { get; set; }
    }
}
