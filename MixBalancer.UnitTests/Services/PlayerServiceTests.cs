using MixBalancer.Application.Dtos.Player;
using MixBalancer.Application.Services.Players;
using MixBalancer.Domain.Entities;
using MixBalancer.Domain.Interfaces;
using Moq;

namespace MixBalancer.UnitTests.Services.Players
{
    public class PlayerServiceTests
    {
        private readonly Mock<IPlayerRepository> _playerRepositoryMock;
        private readonly PlayerService _playerService;

        public PlayerServiceTests()
        {
            _playerRepositoryMock = new Mock<IPlayerRepository>();
            _playerService = new PlayerService(_playerRepositoryMock.Object);
        }

        [Fact]
        public async Task CreatePlayerAsync_ShouldReturnSuccess_WhenPlayerIsCreated()
        {
            // Arrange
            var createPlayerDto = new CreatePlayerDto { NickName = "Player1", SkillLevel = 1 };

            // Act
            var result = await _playerService.CreatePlayerAsync(createPlayerDto);

            // Assert
            Assert.True(result.IsSuccess);
        }

        // Additional tests for GetAllPlayersAsync, UpdatePlayerAsync, etc.
        [Fact]
        public async Task UpdatePlayerAsync_ShouldReturnSuccess_WhenPlayerIsUpdated()
        {
            // Arrange
            var playerId = Guid.NewGuid();
            var updatePlayerDto = new UpdatePlayerDto { NickName = "Player1", SkillLevel = 1 };
            _playerRepositoryMock.Setup(repo => repo.GetByIdAsync(playerId)).ReturnsAsync(new Player());
            // Act
            var result = await _playerService.UpdatePlayerAsync(playerId, updatePlayerDto);
            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task UpdatePlayerAsync_ShouldReturnFailed_WhenPlayerNotFound()
        {
            // Arrange
            var playerId = Guid.NewGuid();
            var updatePlayerDto = new UpdatePlayerDto { NickName = "Player1", SkillLevel = 1 };
            _playerRepositoryMock.Setup(repo => repo.GetByIdAsync(playerId)).ReturnsAsync((Player)null);
            // Act
            var result = await _playerService.UpdatePlayerAsync(playerId, updatePlayerDto);
            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Player not found", result.ErrorMessage);
        }

        [Fact]
        public async Task DeletePlayerAsync_ShouldReturnSuccess_WhenPlayerIsDeleted()
        {
            // Arrange
            var playerId = Guid.NewGuid();
            _playerRepositoryMock.Setup(repo => repo.GetByIdAsync(playerId)).ReturnsAsync(new Player());
            // Act
            var result = await _playerService.DeletePlayerAsync(playerId);
            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task DeletePlayerAsync_ShouldReturnFailed_WhenPlayerNotFound()
        {
            // Arrange
            var playerId = Guid.NewGuid();
            _playerRepositoryMock.Setup(repo => repo.GetByIdAsync(playerId)).ReturnsAsync((Player)null);
            // Act
            var result = await _playerService.DeletePlayerAsync(playerId);
            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Player not found", result.ErrorMessage);
        }

        [Fact]
        public async Task GetAllPlayersAsync_ShouldReturnSuccess_WhenPlayersAreReturned()
        {
            // Arrange
            _playerRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<Player> { new Player() });
            // Act
            var result = await _playerService.GetAllPlayersAsync();
            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotEmpty(result.Data);
        }

        [Fact]
        public async Task GetAllPlayersAsync_ShouldReturnSuccess_WhenNoPlayersAreReturned()
        {
            // Arrange
            _playerRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<Player>());
            // Act
            var result = await _playerService.GetAllPlayersAsync();
            // Assert
            Assert.True(result.IsSuccess);
            Assert.Empty(result.Data);
        }
    }
}