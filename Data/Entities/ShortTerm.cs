using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Parking_System_API.Data.Entities
{
    public class ShortTerm
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public bool DoProvideFullData { get; set; }

        [Required]
        public bool DoProvideVideo { get; set; }
        [Required]
        public bool DoDetected { get; set; }
        [Required]
        public bool Status
        { //0 : Not Activated , 1: Activated
            get; set;
        }

        //if he comes again with another vehicle >> delete the classifier from face model upon exiting
        public Vehicle Vehicle { get; set; }
        public ICollection<ParkingTransaction> ParkingTransactions { get; set; }

    }
}
