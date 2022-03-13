using Microsoft.IdentityModel.Tokens;
using Parking_System_API.Data.Repositories.ParticipantR;
using Parking_System_API.Data.Repositories.SystemUserR;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Parking_System_API
{
    public class JwtAuthenticationManager
    {
        private readonly ISystemUserRepository systemUserRepository;
        private readonly IParticipantRepository participantRepository;

        public JwtAuthenticationManager(ISystemUserRepository systemUserRepository, IParticipantRepository participantRepository)
        {
            this.systemUserRepository = systemUserRepository;
            this.participantRepository = participantRepository;
        }
        public async Task<JwtAuthenticationResponse> AuthenticateCustomer(string email, string password)
        {
            //Validating the User Name and Password
            var participant = await participantRepository.GetParticipantAsyncByEmail(email);

            if(participant == null)
            {
                return null;
            }
            string generatedHashed = Hashing.HashingClass.GenerateHashedPassword(password, participant.Salt);

            if(generatedHashed != participant.Password)
            {
                return null;
            }

            var tokenExpiryTimeStamp = DateTime.Now.AddMinutes(Constants.JWT_TOKEN_VALIDITY_MINS);
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(Constants.JWT_SECURITY_KEY);
            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Email, email),
                    new Claim("ParticipantID", participant.Id),
                    new Claim(ClaimTypes.Role, "participant")
                  
                }),
                Expires = tokenExpiryTimeStamp,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
            var token = jwtSecurityTokenHandler.WriteToken(securityToken);

            return new JwtAuthenticationResponse
            {
                token = token,
                expires_in = (int)tokenExpiryTimeStamp.Subtract(DateTime.Now).TotalSeconds
            };
        }


        public async Task<JwtAuthenticationResponse> AuthenticateAdminAndOpertor(string email, string password)
        {

            //Validating the User Name and Password
            var systemUser = await systemUserRepository.GetSystemUserAsyncByEmail(email);

            if (systemUser == null)
            {
                return null;
            }
            string generatedHashed = Hashing.HashingClass.GenerateHashedPassword(password, systemUser.Salt);

            if (generatedHashed != systemUser.Password)
            {
                return null;
            }
            var role = "";

            if (systemUser.IsAdmin)
            {
                role = "admin";
            }
            else
            {
                role = "operator";
            }

            var tokenExpiryTimeStamp = DateTime.Now.AddMinutes(Constants.JWT_TOKEN_VALIDITY_MINS);
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(Constants.JWT_SECURITY_KEY);
            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Email,email),
                    new Claim(ClaimTypes.Role, role)

                }),
                Expires = tokenExpiryTimeStamp,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
            var token = jwtSecurityTokenHandler.WriteToken(securityToken);

            return new JwtAuthenticationResponse
            {
                token = token,
                expires_in = (int)tokenExpiryTimeStamp.Subtract(DateTime.Now).TotalSeconds
            };
        }

    }
}
