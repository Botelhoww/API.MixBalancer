using Microsoft.Extensions.Configuration;
using MixBalancer.Application.Dtos;
using MixBalancer.Application.Services;
using MixBalancer.Domain.Entities;
using MixBalancer.Domain.Interfaces;
using Moq;

namespace MixBalancer.Tests.Services
{
    public class AuthServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IPlayerRepository> _playerRepositoryMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _playerRepositoryMock = new Mock<IPlayerRepository>();
            _configurationMock = new Mock<IConfiguration>();
            _authService = new AuthService(_userRepositoryMock.Object, _configurationMock.Object, _playerRepositoryMock.Object);
        }

        [Fact]
        public async Task RegisterAsync_EmailAlreadyExists_ReturnsFailedResult()
        {
            // Arrange
            var registerDto = new RegisterDto { Email = "test@example.com", Password = "Password123", Username = "TestUser" };
            _userRepositoryMock.Setup(repo => repo.EmailExistsAsync(registerDto.Email)).ReturnsAsync(true);

            // Act
            var result = await _authService.RegisterAsync(registerDto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Email já cadastrado", result.ErrorMessage);
        }

        [Fact]
        public async Task LoginAsync_InvalidCredentials_ReturnsFailedResult()
        {
            // Arrange
            var loginDto = new LoginDto { Email = "test@example.com", Password = "WrongPassword" };
            _userRepositoryMock.Setup(repo => repo.GetByEmailAsync(loginDto.Email)).ReturnsAsync((User)null);

            // Act
            var result = await _authService.LoginAsync(loginDto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Credenciais inválidas", result.ErrorMessage);
        }

        // Additional tests for successful registration and login can be added here
    }
}