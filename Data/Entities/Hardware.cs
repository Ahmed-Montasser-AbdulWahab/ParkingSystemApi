using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Parking_System_API.Data.Entities
{

    public class Hardware
    {
        [Required, Key]
        public int HardwareId { get; set; }
        [Required]
        public string HardwareType { get; set; }
        
        public string ConnectionString { get; set; }

        [Required]
        public bool Service { get; set; }
        
        public bool Direction { get; set; }

        public ICollection<ParkingTransaction> ParkingTransactions { get; set; }

        
    }
}
