using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Parking_System_API.Data.Entities
{
    public class Terminal
    {
        [Required, Key]
        public int Id { get; set; }
        public string ConnectionString { get; set; } //Wired or Network
        [Required]
        public bool Service { get; set; } //Online 1 or Offline 0
        [Required]
        public bool Direction { get; set; }   //Entry 1 or Exit 0
        //[ForeignKey("FK_Terminal_Terminal")]
        //public int FRCameraId { get; set; }
        //[ForeignKey("FK_Gate_Terminal")]
        //public int LPCameraId { get; set; }



        //public virtual Camera FRCamera { get; set; }
        //public virtual Camera LPCamera { get; set; }
        //public virtual Gate Gate { get; set; }
        public virtual ICollection<ParkingTransaction> ParkingTransactions { get; set; }
    }
}
