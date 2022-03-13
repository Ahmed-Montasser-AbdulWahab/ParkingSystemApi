using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Parking_System_API.Data.Models
{
    public class UploadMyPicture
    {

        [Required]
        public IFormFile Picture { get; set; }
    }
}
