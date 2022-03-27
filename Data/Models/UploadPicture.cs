using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Parking_System_API.Data.Models
{
    public class UploadMedia
    {

        [Required]
        public IFormFile Media { get; set; }
    }
}
