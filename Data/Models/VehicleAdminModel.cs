using System;
using System.ComponentModel.DataAnnotations;

namespace Parking_System_API.Data.Models
{
    public class VehicleAdminModel
    {
        [Required]
        public string PlateNumberId { get; set; }

        public string BrandName { get; set; }
        public string SubCategory { get; set; }

        public string Color { get; set; }

        public DateTime? StartSubscription { get; set; }

        public DateTime? EndSubscription { get; set; }
    }
}
