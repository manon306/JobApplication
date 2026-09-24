using JobApplication.DataModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobApplication.Application.interfaces
{
    public interface IAuthRepo
    {
        public Task<bool> RegisterAsync(ApplicationUser user, string password);
        Task<ApplicationUser?> LoginAsync(string email, string password);
        Task SaveRefreshTokenAsync(RefreshToken refreshToken);
        Task<RefreshToken?> GetRefreshTokenAsync(string token);
        Task LogoutAsync(string refreshToken);
        Task CreateCandidateAsync(Candidate candidate);
        Task DeleteExpiredRefreshTokensAsync();
    }
}
