namespace MixBalancer.Domain.Entities
{
    public class Team
    {
        public Guid Id { get; set; }

        // Relacionamento com Players
        public List<Player> Players { get; set; }

        public decimal AverageSkillLevel { get; set; }

        // Opcional: Gerente do time (User)
        public Guid? ManagedByUserId { get; set; }
        public User ManagedByUser { get; set; }

        // Relacionamento com Matches
        public List<Match> MatchesAsTeamA { get; set; } // Time jogando como TeamA
        public List<Match> MatchesAsTeamB { get; set; } // Time jogando como TeamB
    }
}