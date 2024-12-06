using MixBalancer.Domain.Enums;

namespace MixBalancer.Application.DTOs
{
    public class UpdateMatchDto
    {
        public MatchStatus Status { get; set; }
        public int? ScoreTeamA { get; set; }
        public int? ScoreTeamB { get; set; }
    }
}