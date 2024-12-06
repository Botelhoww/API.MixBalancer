using MixBalancer.Application.Dtos.Team;
using MixBalancer.Domain.Enums;

namespace MixBalancer.Application.DTOs
{
    public class MatchResultDto
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public MatchStatus Status { get; set; }
        public int? ScoreTeamA { get; set; }
        public int? ScoreTeamB { get; set; }

        public TeamResultDto TeamA { get; set; }
        public TeamResultDto TeamB { get; set; }
    }
}