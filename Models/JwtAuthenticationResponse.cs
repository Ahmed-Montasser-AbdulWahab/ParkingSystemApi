using System;

namespace Parking_System_API
{
    [Serializable]
    public class JwtAuthenticationResponse
    {     
            public string token { get; set; }
            public int expires_in { get; set; }
    }
    
}
