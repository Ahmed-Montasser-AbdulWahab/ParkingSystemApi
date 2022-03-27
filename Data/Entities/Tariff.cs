using System.ComponentModel.DataAnnotations;

namespace Parking_System_API.Data.Entities
{
    public class Tariff
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public double CostUnit { get; set; }

    }
}
