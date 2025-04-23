using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Backend.Data;
using Backend.DL;
using Backend.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Backend.BL
{
    public class AuthBL : IAuthBL
    {
        private readonly IAuthDL _authDL;
        private readonly IConfiguration _configuration;

        public AuthBL(IAuthDL authDL, IConfiguration configuration)
        {
            _authDL = authDL;
            _configuration = configuration;
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest model)
        {
            // Kiểm tra user đã tồn tại
            var existingUser = await _authDL.FindUserByUsernameAsync(model.Username);
            if (existingUser != null)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = "Tên đăng nhập đã tồn tại!"
                };
            }

            // Tạo user mới
            var user = new ApplicationUser
            {
                UserName = model.Username,
                Email = model.Email,
                FullName = model.FullName,
                CreatedAt = DateTime.UtcNow
            };

            var isCreated = await _authDL.CreateUserAsync(user, model.Password);

            if (!isCreated)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = "Đăng ký không thành công!"
                };
            }

            // Tạo JWT token
            var token = await GenerateJwtToken(user);
            var refreshToken = await _authDL.CreateRefreshTokenAsync(user.Id);

            return new AuthResponse
            {
                Success = true,
                Message = "Đăng ký thành công!",
                Token = token,
                RefreshToken = refreshToken.Token,
                Expiration = DateTime.UtcNow.AddMinutes(double.Parse(_configuration["JWT:TokenValidityInMinutes"] ?? "60")),
                User = MapToUserDetail(user)
            };
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest model)
        {
            // Tìm user
            var user = await _authDL.FindUserByUsernameAsync(model.Username);
            if (user == null)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = "Tên đăng nhập không tồn tại!"
                };
            }

            // Kiểm tra password
            var isPasswordValid = await _authDL.CheckPasswordAsync(user, model.Password);
            if (!isPasswordValid)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = "Mật khẩu không chính xác!"
                };
            }

            // Cập nhật trạng thái đăng nhập
            user.IsOnline = true;
            user.LastLogin = DateTime.UtcNow;

            // Tạo JWT token
            var token = await GenerateJwtToken(user);
            var refreshToken = await _authDL.CreateRefreshTokenAsync(user.Id);

            return new AuthResponse
            {
                Success = true,
                Message = "Đăng nhập thành công!",
                Token = token,
                RefreshToken = refreshToken.Token,
                Expiration = DateTime.UtcNow.AddMinutes(double.Parse(_configuration["JWT:TokenValidityInMinutes"] ?? "60")),
                User = MapToUserDetail(user)
            };
        }

        public async Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest model)
        {
            // Tìm refresh token
            var refreshToken = await _authDL.GetRefreshTokenAsync(model.RefreshToken);
            if (refreshToken == null || refreshToken.ExpiryDate < DateTime.UtcNow)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = "Refresh token không hợp lệ hoặc đã hết hạn!"
                };
            }

            // Lấy thông tin user
            var user = refreshToken.User;
            if (user == null)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = "Không tìm thấy thông tin người dùng!"
                };
            }

            // Đánh dấu refresh token hiện tại đã sử dụng
            refreshToken.IsRevoked = true;
            await _authDL.UpdateRefreshTokenAsync(refreshToken);

            // Tạo token mới
            var newToken = await GenerateJwtToken(user);
            var newRefreshToken = await _authDL.CreateRefreshTokenAsync(user.Id);

            return new AuthResponse
            {
                Success = true,
                Message = "Token làm mới thành công!",
                Token = newToken,
                RefreshToken = newRefreshToken.Token,
                Expiration = DateTime.UtcNow.AddMinutes(double.Parse(_configuration["JWT:TokenValidityInMinutes"] ?? "60")),
                User = MapToUserDetail(user)
            };
        }

        public async Task<bool> RevokeTokenAsync(string userId)
        {
            // Thực hiện logic revoke token
            // Có thể thực hiện sau
            return true;
        }

        private async Task<string> GenerateJwtToken(ApplicationUser user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"] ?? ""));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName ?? ""),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("userId", user.Id)
            };

            var tokenValidityInMinutes = double.Parse(_configuration["JWT:TokenValidityInMinutes"] ?? "60");
            var tokenExpiration = DateTime.UtcNow.AddMinutes(tokenValidityInMinutes);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                claims: claims,
                expires: tokenExpiration,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private UserDetail MapToUserDetail(ApplicationUser user)
        {
            return new UserDetail
            {
                Id = user.Id,
                Username = user.UserName ?? "",
                Email = user.Email ?? "",
                FullName = user.FullName,
                IsOnline = user.IsOnline
            };
        }
    }
}
