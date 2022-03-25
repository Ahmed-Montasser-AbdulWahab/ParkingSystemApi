using System.ComponentModel.DataAnnotations;

namespace Parking_System_API.Data.Entities
{
    public class Tariff
    {
        public int Id { get; set; }
        
        public string Type { get; set; }

        public double CostUnit { get; set; }

    }
}
