using Backend.Data;
using Backend.Models;

namespace Backend.DL
{
    public interface IAuthDL
    {
        Task<ApplicationUser?> FindUserByUsernameAsync(string username);
        Task<ApplicationUser?> FindUserByIdAsync(string userId);
        Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
        Task<bool> CreateUserAsync(ApplicationUser user, string password);
        Task<RefreshToken> CreateRefreshTokenAsync(string userId);
        Task<RefreshToken?> GetRefreshTokenAsync(string token);
        Task<bool> UpdateRefreshTokenAsync(RefreshToken refreshToken);
    }
}
