using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Parking_System_API.Data.Entities
{
    public class Vehicle
    {
        [Key, Required]
        public string PlateNumberId { get; set; }

        public string BrandName { get; set; }
        public string SubCategory { get; set; }
  
        public string Color { get; set; }

        public DateTime StartSubscription { get; set; }

        public DateTime EndSubscription { get; set; }
        [Required]
        public bool IsPresent { get; set; }
        [Required]
        public bool IsActive { get; set; }

        public ICollection<Participant> Participants { get; set; }
        public ICollection<ParkingTransaction> ParkingTransactions { get; set; }


    }
}
