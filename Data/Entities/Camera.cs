using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Parking_System_API.Data.Entities
{

    public class Camera
    {
        [Required, Key]
        public int Id { get; set; }
        [Required]
        public string CameraType { get; set; }
        
        public string ConnectionString { get; set; }

        [Required]
        public bool Service { get; set; }
        [ForeignKey("FK_Camera_Terminal")]
        public int TerminalId { get; set; }
        public virtual Terminal Terminal { get; set; }
        


        
    }
}
