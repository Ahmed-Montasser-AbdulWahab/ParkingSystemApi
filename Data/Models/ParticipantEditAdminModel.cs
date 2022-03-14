using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Parking_System_API.Data.Models
{
    public class ParticipantEditAdminModel
    {   [Required]
        public long? NationalId { get; set; }
        [Required]
        public bool IsEgyptian { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public IList<string> PlateNumberIds { get; set; }


    }
}
