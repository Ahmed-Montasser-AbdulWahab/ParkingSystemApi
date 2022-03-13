using Microsoft.AspNetCore.Http;
using Parking_System_API.Data.Models;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Parking_System_API.Helper
{
    public class FaceDetectionApi
    {
        public static async Task<string> Detect(long id, IFormFile picture)
        {

            var url = "http://localhost:8001/face_saving";


            var user = new UploadPicture
            {
                Picture = picture
            };
            var json = JsonSerializer.Serialize(user);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            using var client = new HttpClient();

            var response = await client.PostAsync(url, data);

            string result = response.Content.ReadAsStringAsync().Result;

            return result;
        }
    }
}
