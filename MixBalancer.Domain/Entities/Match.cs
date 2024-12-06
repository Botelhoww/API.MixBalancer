using MixBalancer.Domain.Enums;

namespace MixBalancer.Domain.Entities
{
    public class Match
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public Team TeamA { get; set; }
        public Team TeamB { get; set; }
        public MatchStatus Status { get; set; }
        public int ScoreTeamA { get; set; }
        public int ScoreTeamB { get; set; }
    }
}
