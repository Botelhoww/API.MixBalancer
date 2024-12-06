namespace MixBalancer.Domain.Entities
{
    public class Team
    {
        public Guid Id { get; set; }
        public List<Player> Players { get; set; }
        public decimal AverageSkillLevel { get; set; }
    }
}
