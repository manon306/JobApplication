using JobApplication.Application.DTOs;

namespace JobApplication.Application.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(LoginDto dto);
        Task<AuthResponseDto> RegisterAsync(RegisterDto dto);
        Task<AuthResponseDto> RefreshTokenAsync(RefreshRequestDto dto);
        Task LogoutAsync(string refreshToken);
    }
}
