﻿using Microsoft.IdentityModel.Tokens;
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

        public JwtAuthenticationManager(ISystemUserRepository systemUserRepository)
        {
            this.systemUserRepository = systemUserRepository;
        }
        public async Task<JwtAuthenticationResponse> AuthenticateCustomer(string email, string password)
        {
            //Validating the User Name and Password
            var systemUser = await systemUserRepository.GetSystemUserAsyncByEmail(email);

            if(systemUser == null)
            {
                return null;
            }
            string generatedHashed = Hashing.HashingClass.GenerateHashedPassword(password, systemUser.Salt);

            if(generatedHashed == systemUser.Password)
            {
                return null;
            }

            var tokenExpiryTimeStamp = DateTime.Now.AddMinutes(Constants.JWT_TOKEN_VALIDITY_MINS);
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(Constants.JWT_SECURITY_KEY);
            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new List<Claim>
                {
                    new Claim("email", email),
                    new Claim(ClaimTypes.Role, "CUSTOMER")
                  
                }),
                Expires = tokenExpiryTimeStamp,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
            var token = jwtSecurityTokenHandler.WriteToken(securityToken);

            return new JwtAuthenticationResponse
            {
                token = token,
                Email = email,
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

            if (systemUser.Type)
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
                Subject = new System.Security.Claims.ClaimsIdentity(new List<Claim>
                {
                    new Claim("email",email),
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
                Email = email,
                expires_in = (int)tokenExpiryTimeStamp.Subtract(DateTime.Now).TotalSeconds
            };
        }

    }
}
