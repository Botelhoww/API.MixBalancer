namespace MixBalancer.Domain.Entities
{
    public class MatchHistory
    {
        public Guid Id { get; set; }
        public Guid PlayerId { get; set; }
        public Player Player { get; set; }

        public DateTime Date { get; set; }
        public string Map { get; set; }
        public string Result { get; set; } // Vitória ou Derrota
        public double KD { get; set; }
    }
}