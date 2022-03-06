namespace Parking_System_API.Model
{
    [System.Serializable]
    public class AuthenticationRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
