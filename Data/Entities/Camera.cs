using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Parking_System_API.Data.Entities
{

    public class Camera
    {
        [Required, Key]
        public int CameraId { get; set; }
        [Required]
        public string CameraType { get; set; }
        
        public string ConnectionString { get; set; }

        [Required]
        public bool Service { get; set; }
        


        
    }
}
