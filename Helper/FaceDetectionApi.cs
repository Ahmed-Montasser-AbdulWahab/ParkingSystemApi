using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
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
        public static string Detect(string id, string picture)
        {
            var url = $"http://localhost:8001/face_saving?Id={id}";
            WebClient client = new WebClient();
            byte[] response =  client.UploadFile(url, picture);
            //HttpClient httpClient = new HttpClient();
            //MultipartFormDataContent form = new MultipartFormDataContent();

            //form.Add(new StringContent(id), "Id");
            //var FileStream = new FileStream(picture, FileMode.Open);
            //form.Add(new StreamContent(FileStream), @"file1");
            //var data = form.ReadAsStreamAsync();
            //HttpResponseMessage response = await httpClient.PostAsync(url, form);

            //response.EnsureSuccessStatusCode();
            //httpClient.Dispose();
            //string sd = response.Content.ReadAsStringAsync().Result;

            //var user = new
            //{
            //    Id = id,
            //    file1 = picture
            //};
            //var json = JsonSerializer.Serialize(user);
            //var data = new StringContent(json, Encoding.UTF8, "application/json");

            //using var client = new HttpClient();

            //var response = await client.PostAsync(url, data);

            //string result = response.Content.ReadAsStringAsync().Result;
            string res = System.Text.Encoding.ASCII.GetString(response);
            
            return res;
        }
    }
}
