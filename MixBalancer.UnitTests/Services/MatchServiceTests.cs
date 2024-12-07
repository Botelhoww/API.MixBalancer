using MixBalancer.Application.Dtos.Match;
using MixBalancer.Application.DTOs;
using MixBalancer.Application.Services;
using MixBalancer.Domain.Enums;
using MixBalancer.Domain.Interfaces;
using Moq;

namespace MixBalancer.UnitTests.Services
{
    public class MatchServiceTests
    {
        private readonly Mock<IMatchRepository> _matchRepositoryMock;
        private readonly Mock<ITeamRepository> _teamRepositoryMock;
        private readonly MatchService _matchService;

        public MatchServiceTests()
        {
            _matchRepositoryMock = new Mock<IMatchRepository>();
            _teamRepositoryMock = new Mock<ITeamRepository>();
            _matchService = new MatchService(_matchRepositoryMock.Object, _teamRepositoryMock.Object);
        }

        [Fact]
        public async Task CreateMatchAsync_ShouldReturnFailed_WhenTeamsNotFound()
        {
            // Arrange
            var createMatchDto = new CreateMatchDto { TeamAId = Guid.NewGuid(), TeamBId = Guid.NewGuid() };
            _teamRepositoryMock.Setup(repo => repo.GetByIdAsync(createMatchDto.TeamAId)).ReturnsAsync((MixBalancer.Domain.Entities.Team)null);
            _teamRepositoryMock.Setup(repo => repo.GetByIdAsync(createMatchDto.TeamBId)).ReturnsAsync((MixBalancer.Domain.Entities.Team)null);

            // Act
            var result = await _matchService.CreateMatchAsync(createMatchDto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("One or both teams not found.", result.ErrorMessage);
        }

        // Additional tests for GetMatchByIdAsync, UpdateMatchAsync, etc.
        [Fact]
        public async Task CreateMatchAsync_ShouldReturnSuccess_WhenMatchIsCreated()
        {
            // Arrange
            var createMatchDto = new CreateMatchDto { TeamAId = Guid.NewGuid(), TeamBId = Guid.NewGuid() };
            _teamRepositoryMock.Setup(repo => repo.GetByIdAsync(createMatchDto.TeamAId)).ReturnsAsync(new MixBalancer.Domain.Entities.Team());
            _teamRepositoryMock.Setup(repo => repo.GetByIdAsync(createMatchDto.TeamBId)).ReturnsAsync(new MixBalancer.Domain.Entities.Team());
            // Act
            var result = await _matchService.CreateMatchAsync(createMatchDto);
            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task GetMatchByIdAsync_ShouldReturnFailed_WhenMatchNotFound()
        {
            // Arrange
            var matchId = Guid.NewGuid();
            _matchRepositoryMock.Setup(repo => repo.GetByIdAsync(matchId)).ReturnsAsync((MixBalancer.Domain.Entities.Match)null);
            // Act
            var result = await _matchService.GetMatchByIdAsync(matchId);
            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Match not found.", result.ErrorMessage);
        }

        [Fact]
        public async Task GetMatchByIdAsync_ShouldReturnSuccess_WhenMatchIsFound()
        {
            // Arrange
            var matchId = Guid.Parse("c15d6ae3-95f9-4c9a-9909-ec098b6d989c");
            _matchRepositoryMock.Setup(repo => repo.GetByIdAsync(matchId)).ReturnsAsync(new MixBalancer.Domain.Entities.Match());

            // Act
            var result = await _matchService.GetMatchByIdAsync(matchId);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task UpdateMatchAsync_ShouldReturnFailed_WhenMatchNotFound()
        {
            // Arrange
            var matchId = Guid.NewGuid();
            var updateMatchDto = new UpdateMatchDto();
            _matchRepositoryMock.Setup(repo => repo.GetByIdAsync(matchId)).ReturnsAsync((MixBalancer.Domain.Entities.Match)null);

            // Act
            var result = await _matchService.UpdateMatchAsync(matchId, updateMatchDto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Match not found.", result.ErrorMessage);
        }

        [Fact]
        public async Task UpdateMatchAsync_ShouldReturnSuccess_WhenMatchIsUpdated()
        {
            // Arrange
            var matchId = new Guid();
            matchId = Guid.Parse("c15d6ae3-95f9-4c9a-9909-ec098b6d989c");

            var updateMatchDto = new UpdateMatchDto();
            _matchRepositoryMock.Setup(repo => repo.GetByIdAsync(matchId)).ReturnsAsync(new MixBalancer.Domain.Entities.Match());
            
            // Act
            var result = await _matchService.UpdateMatchAsync(matchId, updateMatchDto);
            
            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task UpdateMatchAsync_ShouldReturnFailed_WhenMatchIsFinished()
        {
            // Arrange
            var matchId = Guid.NewGuid();
            var updateMatchDto = new UpdateMatchDto { ScoreTeamA = 13, ScoreTeamB = 3, Status = MatchStatus.Finished };
            _matchRepositoryMock.Setup(repo => repo.GetByIdAsync(matchId)).ReturnsAsync(new MixBalancer.Domain.Entities.Match { Status = MatchStatus.Finished });

            // Act
            var result = await _matchService.UpdateMatchAsync(matchId, updateMatchDto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Unable to update. Match is already finished.", result.ErrorMessage);
        }

        [Fact]
        public async Task UpdateMatchAsync_ShouldReturnFailed_WhenMatchIsCancelled()
        {
            // Arrange
            var matchId = Guid.NewGuid();
            var updateMatchDto = new UpdateMatchDto { ScoreTeamA = 13, ScoreTeamB = 3, Status = MatchStatus.Cancelled };
            _matchRepositoryMock.Setup(repo => repo.GetByIdAsync(matchId)).ReturnsAsync(new MixBalancer.Domain.Entities.Match { Status = MatchStatus.Cancelled });

            // Act
            var result = await _matchService.UpdateMatchAsync(matchId, updateMatchDto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Unable to update. Match is cancelled.", result.ErrorMessage);
        }     
    }
}