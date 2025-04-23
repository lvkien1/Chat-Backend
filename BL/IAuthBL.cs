using Backend.Models;

namespace Backend.BL
{
    public interface IAuthBL
    {
        Task<AuthResponse> RegisterAsync(RegisterRequest model);
        Task<AuthResponse> LoginAsync(LoginRequest model);
        Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest model);
        Task<bool> RevokeTokenAsync(string userId);
    }
}
