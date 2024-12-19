namespace MixBalancer.Domain.Entities
{
    public class Player
    {
        public Guid Id { get; set; }

        // Relacionamento com User
        public Guid UserId { get; set; }
        public User User { get; set; }

        public string Nickname { get; set; }
        public int SkillLevel { get; set; }

        // Estatísticas
        public double WinRate { get; set; }
        public double KDRatio { get; set; }
        public double HeadshotPercentage { get; set; }
        public int TotalMatches { get; set; }
        public string BestMap { get; set; }
        public string WorstMap { get; set; }
        public int Aces { get; set; }
        public int Clutches { get; set; }


        // Relacionamento com MatchHistory
        public List<MatchHistory> MatchHistories { get; set; }

        // Relacionamento com Teams
        public List<Team> Teams { get; set; }

        // Relacionamento com Matches (via Team)
        public List<Match> Matches { get; set; }
    }
}