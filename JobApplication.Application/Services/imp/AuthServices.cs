using JobApplication.Application.DTOs;
using JobApplication.Application.interfaces;
using JobApplication.DataModel.Constants;
using JobApplication.DataModel.Entities;
using Microsoft.AspNetCore.Identity;

namespace JobApplication.Application.Services.imp
{
    public class AuthServices : IAuthService
    {
        private readonly IAuthRepo _authRepo;
        private readonly IJwtService _jwtService;
        private readonly UserManager<ApplicationUser> _userManager;
        public AuthServices(IAuthRepo authRepo, IJwtService jwtService, UserManager<ApplicationUser> userManager)
        {
            _authRepo = authRepo;
            _jwtService = jwtService;
            _userManager = userManager;
        }
        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _authRepo.LoginAsync(dto.Email, dto.Password);
            var roles = await _userManager.GetRolesAsync(user);
            var accessToken = _jwtService.GenerateToken(user.Id, user.Email!,roles);

            var refreshToken = _jwtService.GenerateRefreshToken();

            var refreshTokenEntity = new RefreshToken
            {
                Token = refreshToken,
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            };

            await _authRepo.SaveRefreshTokenAsync(refreshTokenEntity);

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
            var result = await _authRepo.RegisterAsync(user, dto.Password);

            if (result != true)
            {
                throw new Exception("User registration failed.");
            }

            var roleResult = await _userManager.AddToRoleAsync(user, Roles.Candidate);

            if (!roleResult.Succeeded)
            {
                throw new Exception("Failed to assign Candidate role.");
            }
            var roles = await _userManager.GetRolesAsync(user);
            await _authRepo.CreateCandidateAsync(new Candidate
            {
                UserId = user.Id,
                Name = dto.FullName,
                Email = dto.Email
            });
            var accessToken = _jwtService.GenerateToken(user.Id, user.Email!, roles);
            var refreshToken = _jwtService.GenerateRefreshToken();
            var refreshTokenEntity = new RefreshToken
            {
                Token = refreshToken,
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            };
            await _authRepo.SaveRefreshTokenAsync(refreshTokenEntity);
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
            var roles = await _userManager.GetRolesAsync(user);
            var accessToken = _jwtService.GenerateToken(
                user.Id,
                user.Email!
                , roles);

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
        public async Task LogoutAsync(LogoutRequestDto dto)
        {
             await _authRepo.LogoutAsync(dto.RefreshToken);
        }
    }
}
