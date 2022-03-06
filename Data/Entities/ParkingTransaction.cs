using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Parking_System_API.Data.Entities
{
    public class ParkingTransaction
    {   [Key, Required, ForeignKey("FK_Participant_RelatedTo")]
        public long ParticipantId { get; set; }
        [Key, Required, ForeignKey("FK_Vehicle_RelatedTo")]
        public string PlateNumberId { get; set; }
        [Key, Required, ForeignKey("FK_Hardware_RelatedTo")]
        public int HardwareId { get; set; }
        [Key, Required]
        public DateTime DateTimeTransaction { get; set; }
        [Required]
        public bool Result { get; set; } //0: Fail , 1: Succeed


        public Participant participant { get; set; }
        public Vehicle vehicle { get; set; }
        public Hardware hardware { get; set; }
    }
}
