using MixBalancer.Application.Dtos.Player;

namespace MixBalancer.Application.Dtos.Team
{
    public class TeamResultDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<PlayerResultDto> Players { get; set; }
        public decimal AverageSkillLevel { get; set; }
        public Guid? ManagedByUserId { get; set; }
    }
}