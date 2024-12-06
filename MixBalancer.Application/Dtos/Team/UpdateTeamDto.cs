namespace MixBalancer.Application.Dtos.Team
{
    public class UpdateTeamDto
    {
        public string Name { get; set; }
        public Guid? ManagedByUserId { get; set; }
    }
}