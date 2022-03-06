using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Parking_System_API.Data.Models
{
    public class ParticipantAdminModel
    {
        
        public long? Id { get; set; }
        [Required]
        public bool isEgyptian { get; set; }
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        public IList<string> PlateNumberIds { get; set; }
        public IFormFile ProfileImage { get; set; }

    }
}
