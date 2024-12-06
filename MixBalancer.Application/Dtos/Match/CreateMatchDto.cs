namespace MixBalancer.Application.Dtos.Match
{
    public class CreateMatchDto
    {
        public Guid TeamAId { get; set; }
        public Guid TeamBId { get; set; }
        public DateTime Date { get; set; }
    }
}