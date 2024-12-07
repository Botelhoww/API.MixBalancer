using MixBalancer.Application.Dtos.Team;
using MixBalancer.Application.Services.Team;
using MixBalancer.Domain.Interfaces;
using Moq;

namespace MixBalancer.UnitTests.Services.Team
{
    public class TeamServiceTests
    {
        private readonly Mock<ITeamRepository> _teamRepositoryMock;
        private readonly Mock<IPlayerRepository> _playerRepositoryMock;
        private readonly TeamService _teamService;

        public TeamServiceTests()
        {
            _teamRepositoryMock = new Mock<ITeamRepository>();
            _playerRepositoryMock = new Mock<IPlayerRepository>();
            _teamService = new TeamService(_teamRepositoryMock.Object, _playerRepositoryMock.Object);
        }

        [Fact]
        public async Task CreateTeamAsync_ShouldReturnSuccess_WhenTeamIsCreated()
        {
            // Arrange
            var createTeamDto = new CreateTeamDto { Name = "Team A", ManagedByUserId = Guid.NewGuid() };

            // Act
            var result = await _teamService.CreateTeamAsync(createTeamDto);

            // Assert
            Assert.True(result.IsSuccess);
        }

        // Additional tests for GetAllTeamsAsync, UpdateTeamAsync, etc.
        [Fact]
        public async Task GetAllTeamsAsync_ShouldReturnSuccess_WhenTeamsAreFound()
        {
            // Arrange
            var teams = new List<MixBalancer.Domain.Entities.Team>
            {
                new MixBalancer.Domain.Entities.Team { Id = Guid.NewGuid(), Name = "Team A", ManagedByUserId = Guid.NewGuid(), AverageSkillLevel = 0, Players = new List<MixBalancer.Domain.Entities.Player>() },
                new MixBalancer.Domain.Entities.Team { Id = Guid.NewGuid(), Name = "Team B", ManagedByUserId = Guid.NewGuid(), AverageSkillLevel = 0, Players = new List<MixBalancer.Domain.Entities.Player>() }
            };
            _teamRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(teams);

            // Act
            var result = await _teamService.GetAllTeamsAsync();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(2, result.Data.Count());
        }

        [Fact]
        public async Task UpdateTeamAsync_ShouldReturnFailed_WhenTeamNotFound()
        {
            // Arrange
            var updateTeamDto = new UpdateTeamDto { Name = "Team A", ManagedByUserId = Guid.NewGuid() };
            _teamRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((MixBalancer.Domain.Entities.Team)null);

            // Act
            var result = await _teamService.UpdateTeamAsync(Guid.NewGuid(), updateTeamDto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Team not found", result.ErrorMessage);
        }

        [Fact]
        public async Task UpdateTeamAsync_ShouldReturnSuccess_WhenTeamIsUpdated()
        {
            // Arrange
            var team = new MixBalancer.Domain.Entities.Team { Id = Guid.NewGuid(), Name = "Team A", ManagedByUserId = Guid.NewGuid(), AverageSkillLevel = 0, Players = new List<MixBalancer.Domain.Entities.Player>() };
            var updateTeamDto = new UpdateTeamDto { Name = "Team B", ManagedByUserId = Guid.NewGuid() };
            _teamRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(team);

            // Act
            var result = await _teamService.UpdateTeamAsync(team.Id, updateTeamDto);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task DeleteTeamAsync_ShouldReturnFailed_WhenTeamNotFound()
        {
            // Arrange
            _teamRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((MixBalancer.Domain.Entities.Team)null);

            // Act
            var result = await _teamService.DeleteTeamAsync(Guid.NewGuid());

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Team not found", result.ErrorMessage);
        }

        [Fact]
        public async Task DeleteTeamAsync_ShouldReturnSuccess_WhenTeamIsDeleted()
        {
            // Arrange
            var team = new MixBalancer.Domain.Entities.Team { Id = Guid.NewGuid(), Name = "Team A", ManagedByUserId = Guid.NewGuid(), AverageSkillLevel = 0, Players = new List<MixBalancer.Domain.Entities.Player>() };
            _teamRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(team);

            // Act
            var result = await _teamService.DeleteTeamAsync(team.Id);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task AddPlayerToTeamAsync_ShouldReturnFailed_WhenTeamNotFound()
        {
            // Arrange
            var addPlayerToTeamDto = new AddPlayerToTeamDto { PlayerId = Guid.NewGuid() };
            _teamRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Domain.Entities.Team)null);

            // Act
            var result = await _teamService.AddPlayerToTeamAsync(addPlayerToTeamDto.PlayerId, addPlayerToTeamDto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Team not found", result.ErrorMessage);
        }

        [Fact]
        public async Task AddPlayerToTeamAsync_ShouldReturnFailed_WhenPlayerNotFound()
        {
            // Arrange
            var team = new MixBalancer.Domain.Entities.Team { Id = Guid.NewGuid(), Name = "Team A", ManagedByUserId = Guid.NewGuid(), AverageSkillLevel = 0, Players = new List<MixBalancer.Domain.Entities.Player>() };
            var addPlayerToTeamDto = new AddPlayerToTeamDto { PlayerId = Guid.NewGuid() };
            _teamRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(team);
            _playerRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Domain.Entities.Player)null);

            // Act
            var result = await _teamService.AddPlayerToTeamAsync(team.Id, addPlayerToTeamDto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Player not found", result.ErrorMessage);
        }

        [Fact]
        public async Task AddPlayerToTeamAsync_ShouldReturnSuccess_WhenPlayerIsAddedToTeam()
        {
            // Arrange
            var team = new MixBalancer.Domain.Entities.Team { Id = Guid.NewGuid(), Name = "Team A", ManagedByUserId = Guid.NewGuid(), AverageSkillLevel = 0, Players = new List<MixBalancer.Domain.Entities.Player>() };
            var player = new MixBalancer.Domain.Entities.Player { Id = Guid.NewGuid(), Nickname = "Player1", SkillLevel = 1 };
            var addPlayerToTeamDto = new AddPlayerToTeamDto { PlayerId = player.Id };
            _teamRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(team);
            _playerRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(player);

            // Act
            var result = await _teamService.AddPlayerToTeamAsync(team.Id, addPlayerToTeamDto);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task RemovePlayerFromTeamAsync_ShouldReturnFailed_WhenTeamNotFound()
        {
            // Arrange
            var removePlayerFromTeamDto = new RemovePlayerFromTeamDto { PlayerId = Guid.NewGuid() };
            _teamRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Domain.Entities.Team)null);

            // Act
            var result = await _teamService.RemovePlayerFromTeamAsync(removePlayerFromTeamDto.PlayerId, removePlayerFromTeamDto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Team not found", result.ErrorMessage);
        }

        [Fact]
        public async Task RemovePlayerFromTeamAsync_ShouldReturnFailed_WhenPlayerNotFound()
        {
            // Arrange
            var team = new MixBalancer.Domain.Entities.Team { Id = Guid.NewGuid(), Name = "Team A", ManagedByUserId = Guid.NewGuid(), AverageSkillLevel = 0, Players = new List<MixBalancer.Domain.Entities.Player>() };
            var removePlayerFromTeamDto = new RemovePlayerFromTeamDto { PlayerId = Guid.NewGuid() };
            _teamRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(team);
            _playerRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Domain.Entities.Player)null);

            // Act
            var result = await _teamService.RemovePlayerFromTeamAsync(team.Id, removePlayerFromTeamDto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Player not found in team", result.ErrorMessage);
        }

        [Fact]
        public async Task RemovePlayerFromTeamAsync_ShouldReturnSuccess_WhenPlayerIsRemovedFromTeam()
        {
            // Arrange
            var team = new MixBalancer.Domain.Entities.Team { Id = Guid.NewGuid(), Name = "Team A", ManagedByUserId = Guid.NewGuid(), AverageSkillLevel = 0, Players = new List<MixBalancer.Domain.Entities.Player>() };
            var player = new MixBalancer.Domain.Entities.Player { Id = Guid.NewGuid(), Nickname = "Player1", SkillLevel = 1 };
            var removePlayerFromTeamDto = new RemovePlayerFromTeamDto { PlayerId = player.Id };
            team.Players.Add(player);
            _teamRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(team);
            _playerRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(player);

            // Act
            var result = await _teamService.RemovePlayerFromTeamAsync(team.Id, removePlayerFromTeamDto);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task RemovePlayerFromTeamAsync_ShouldReturnFailed_WhenPlayerIsNotInTeam()
        {
            // Arrange
            var team = new MixBalancer.Domain.Entities.Team { Id = Guid.NewGuid(), Name = "Team A", ManagedByUserId = Guid.NewGuid(), AverageSkillLevel = 0, Players = new List<MixBalancer.Domain.Entities.Player>() };
            var player = new MixBalancer.Domain.Entities.Player { Id = Guid.NewGuid(), Nickname = "Player1", SkillLevel = 1 };
            var removePlayerFromTeamDto = new RemovePlayerFromTeamDto { PlayerId = player.Id };
            _teamRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(team);
            _playerRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(player);

            // Act
            var result = await _teamService.RemovePlayerFromTeamAsync(team.Id, removePlayerFromTeamDto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Player not found in team", result.ErrorMessage);
        }
    }
}