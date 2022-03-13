using System;
using System.Security.Cryptography;
namespace Parking_System_API.Helper
{
    public class TokenGeneration
    {
        public static string GenerateToken(int length)
        {
            using (System.Security.Cryptography.RNGCryptoServiceProvider cryptRNG = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                byte[] tokenBuffer = new byte[length];
                cryptRNG.GetBytes(tokenBuffer);
                return Convert.ToBase64String(tokenBuffer);
            }
        }
    }
}
