using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Parking_System_API.Data.Entities
{
    public class Gate
    {
        //ClosedState => 0 ,OpenedState => 1
        [Key, Required]
        public int Id { get; set; }
        [Required]
        public bool Service { get; set; } //Online 1 -- Offline 0
        [Required]
        public bool State { get; set; } //open:1 close:0
        [ForeignKey("FK_Gate_Terminal")]
        public int TerminalId { get; set; }
        public virtual Terminal Terminal { get; set; }
    }
}
