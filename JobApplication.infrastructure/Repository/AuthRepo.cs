
using JobApplication.Application.DTOs;
using JobApplication.Application.interfaces;
using JobApplication.DataModel.Entities;
using JobApplication.infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JobApplication.infrastructure.Repository
{
    public class AuthRepo : IAuthRepo
    {
        private readonly ApplicationDBContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public AuthRepo(ApplicationDBContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<ApplicationUser?> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                throw new Exception("Invalid email or password.");
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(
                user,
                password);

            if (!isPasswordValid)
            {
                throw new Exception("Invalid email or password.");
            }

            return user;
        }
        public async Task<bool> RegisterAsync(ApplicationUser user, string password)
        {
            var result = await _userManager.CreateAsync(user,password);
            if (!result.Succeeded)
            {
                //throw new Exception("User registration failed.");
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception(errors);
            }
            return result.Succeeded;
        }
        public async Task SaveRefreshTokenAsync(RefreshToken refreshToken)
        {
            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();
        }
        public async Task<RefreshToken?> GetRefreshTokenAsync(string token)
        {
            return await _context.RefreshTokens
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Token == token);
        }
        public async Task LogoutAsync(string refreshToken)
        {
            var storedToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(x => x.Token == refreshToken); 
            if (storedToken == null)
            {
                throw new KeyNotFoundException("Refresh token not found.");
            }
            if (storedToken.RevokedAt != null)
            {
                return;
            }
            storedToken.RevokedAt = DateTime.UtcNow; 
            await _context.SaveChangesAsync();
        }
        public async Task CreateCandidateAsync(Candidate candidate)
        {
            _context.Candidates.Add(candidate);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteExpiredRefreshTokensAsync()
        {
            var expiredTokens = await _context.RefreshTokens
                .Where(x => x.ExpiresAt <= DateTime.UtcNow)
                .ToListAsync();

            if (expiredTokens.Count == 0)
                return;

            _context.RefreshTokens.RemoveRange(expiredTokens);
            await _context.SaveChangesAsync();
        }
    }
}
