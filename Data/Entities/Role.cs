using System.ComponentModel.DataAnnotations;

namespace Parking_System_API.Data.Entities
{
    public class Role
    {   [Key]
        public int Id { get; set; }
        [Required]
        public string RoleName { get; set; }
        [Required]
        public string AbbreviationRole { get; set; }
    }
}
