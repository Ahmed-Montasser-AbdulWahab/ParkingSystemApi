using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Parking_System_API.Data.Models
{
    public class UploadPicture
    {

        [Required]
        public IFormFile Picture { get; set; }
    }
}
