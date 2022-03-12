using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Parking_System_API.Data.Models
{
    public class UploadPicture
    {
        [Required]
        public long Id { get; set; }
        [Required]
        public IFormFile pic { get; set; }
    }
}
