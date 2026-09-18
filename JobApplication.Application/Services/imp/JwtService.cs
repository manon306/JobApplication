using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace JobApplication.Application.Services.imp
{
    public class JwtService : IJwtService
    {
        public string GenerateToken(string userId, string email) 
        {
            // Create claims for the token => info in the token
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Email, email)
            };
            // Create a symmetric security key using a secret key
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("ThisIsMySuperSecretKey12345678901")
            );
            // Create signing credentials using the key and the HMAC SHA256 algorithm
            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );
            // Create the JWT token with claims, expiration, and signing credentials
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: credentials
            );
            // Generate the token string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)
            );
        }
    }
}
