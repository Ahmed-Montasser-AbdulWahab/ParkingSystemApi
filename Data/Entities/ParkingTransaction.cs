using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Parking_System_API.Data.Entities
{
    public class ParkingTransaction
    {   [Key, Required, ForeignKey("FK_Participant_RelatedTo")]
        public string ParticipantId { get; set; }
        [Key, Required, ForeignKey("FK_Vehicle_RelatedTo")]
        public string PlateNumberId { get; set; }
        [Key, Required, ForeignKey("FK_Terminal_RelatedTo")]
        public int TerminalId { get; set; }
        [Key, Required]
        public DateTime DateTimeTransaction { get; set; }
        [Required]
        public bool Result { get; set; } //0: Fail , 1: Succeed


        public virtual Participant participant { get; set; }
        public virtual Vehicle vehicle { get; set; }
        public virtual Terminal terminal { get; set; }
    }
}
