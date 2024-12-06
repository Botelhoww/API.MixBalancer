namespace MixBalancer.Domain.Entities
{
    public class Player
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public string Nickname { get; set; }
        public int SkillLevel { get; set; }
        public List<Match> Matches { get; set; }
    }
}
