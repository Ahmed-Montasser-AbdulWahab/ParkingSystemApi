using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Parking_System_API.Data.Entities
{
    public class Participant
    {
        [Key]
        public string Id { get; set; }
        [Required]
        public long NationalId { get; set; }
        [Required]
        public string LastUpdated { get; set; }

        public string Name { get; set; }
//        public string PhoneNumber { get; set; }
        [Required ]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Salt { get; set; }

        public string PhotoUrl { get; set; }
        [Required]
        public bool DoProvideFullData { get; set; }

        [Required]
        public bool DoProvidePhoto { get; set; }
        [Required]
        public bool DoProvideVideo { get; set; }
        [Required]
        public bool DoDetected { get; set; }
        [Required]
        public bool Status { //0 : Not Activated , 1: Activated
            get; set;
        }


        public ICollection<Vehicle> Vehicles { get; set; }
        public ICollection<ParkingTransaction> ParkingTransactions { get; set; }
    }
}
