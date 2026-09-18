using JobApplication.Application.DTOs;
using JobApplication.Application.interfaces;
using JobApplication.DataModel.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobApplication.Application.Services.imp
{
    public class AuthServices : IAuthService
    {
        private readonly IAuthRepo _authRepo;
        private readonly IJwtService _jwtService;
        public AuthServices(IAuthRepo authRepo, IJwtService jwtService)
        {
            _authRepo = authRepo;
            _jwtService = jwtService;
        }
        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _authRepo.LoginAsync(dto.Email, dto.Password);

            var accessToken = _jwtService.GenerateToken(user.Id, user.Email!);

            var refreshToken = _jwtService.GenerateRefreshToken();

            var refreshTokenEntity = new RefreshToken
            {
                Token = refreshToken,
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            };

            _authRepo.SaveRefreshTokenAsync(refreshTokenEntity);

            return new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }
        public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
        {
            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                FullName = dto.FullName
            };
            var result =await _authRepo.RegisterAsync(user, dto.Password);

            var accessToken = _jwtService.GenerateToken(user.Id, user.Email!);
            var refreshToken = _jwtService.GenerateRefreshToken();
            var refreshTokenEntity = new RefreshToken
            {
                Token = refreshToken,
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            };
            _authRepo.SaveRefreshTokenAsync(refreshTokenEntity);
            return new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }
        public async Task<AuthResponseDto> RefreshTokenAsync(RefreshRequestDto dto)
        {
            var storedToken = await _authRepo.GetRefreshTokenAsync(dto.RefreshToken);

            if (storedToken == null)
            {
                throw new UnauthorizedAccessException("Invalid refresh token.");
            }

            if (storedToken.ExpiresAt <= DateTime.UtcNow)
            {
                throw new UnauthorizedAccessException("Refresh token has expired.");
            }

            if (storedToken.RevokedAt != null)
            {
                throw new UnauthorizedAccessException("Refresh token has been revoked.");
            }

            var user = storedToken.User;

            if (user == null)
            {
                throw new UnauthorizedAccessException("User not found.");
            }

            var accessToken = _jwtService.GenerateToken(
                user.Id,
                user.Email!);

            var newRefreshToken = _jwtService.GenerateRefreshToken();

            storedToken.RevokedAt = DateTime.UtcNow;

            var newRefreshTokenEntity = new RefreshToken
            {
                Token = newRefreshToken,
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            };

            await _authRepo.SaveRefreshTokenAsync(newRefreshTokenEntity);

            return new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = newRefreshToken
            };
        }
        public async Task LogoutAsync(string refreshToken)
        {
             await _authRepo.LogoutAsync(refreshToken);
        }
    }
}
