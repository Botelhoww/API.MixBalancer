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

        // Relacionamento com Teams
        public List<Team> Teams { get; set; }

        // Relacionamento com Matches (via Team)
        public List<Match> Matches { get; set; }
    }
}