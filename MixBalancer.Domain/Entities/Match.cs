using MixBalancer.Domain.Enums;

namespace MixBalancer.Domain.Entities
{
    public class Match
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }

        // Relacionamento com Times
        public Guid TeamAId { get; set; }
        public Team TeamA { get; set; }

        public Guid TeamBId { get; set; }
        public Team TeamB { get; set; }

        public MatchStatus Status { get; set; }
        public int? ScoreTeamA { get; set; }
        public int? ScoreTeamB { get; set; }

        // Novo campo para armazenar o MatchId do CS2
        public string MatchIdCS2 { get; set; }  // Campo que armazenará o ID da partida no CS2
        public Guid ManagedByUserId { get; set; }  // Novo campo
    }
}