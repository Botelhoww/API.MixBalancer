namespace MixBalancer.Application.Dtos.Team
{
    public class CreateTeamDto
    {
        public string Name { get; set; }
        public Guid? ManagedByUserId { get; set; }
    }
}