namespace MixBalancer.Application.Dtos.Match
{
    public class AddPlayerDto
    {
        public Guid PlayerId { get; set; }
        public Guid TeamId { get; set; }  // Para indicar a qual time o jogador será adicionado (A ou B)
    }
}