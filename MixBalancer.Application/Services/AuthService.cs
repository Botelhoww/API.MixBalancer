using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MixBalancer.Application.Dtos;
using MixBalancer.Domain.Entities;
using MixBalancer.Domain.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MixBalancer.Application.Services
{
    public class AuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<AuthResult> RegisterAsync(RegisterDto model)
        {
            if (await _userRepository.EmailExistsAsync(model.Email))
                return AuthResult.Failed("Email já cadastrado");

            var salt = BCrypt.Net.BCrypt.GenerateSalt();
            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = model.Username,
                Email = model.Email,
                Salt = salt,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password + salt),
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            await _userRepository.AddUserAsync(user);

            return AuthResult.Success(GenerateJwtToken(user));
        }

        public async Task<AuthResult> LoginAsync(LoginDto model)
        {
            var user = await _userRepository.GetByEmailAsync(model.Email);

            if (user == null || !VerifyPassword(user, model.Password))
                return AuthResult.Failed("Credenciais inválidas");

            user.LastLoginAt = DateTime.UtcNow;
            await _userRepository.UpdateUserAsync(user);

            return AuthResult.Success(GenerateJwtToken(user));
        }

        private bool VerifyPassword(User user, string password)
        {
            return BCrypt.Net.BCrypt.Verify(password + user.Salt, user.PasswordHash);
        }

        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"])
            );
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Username)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
