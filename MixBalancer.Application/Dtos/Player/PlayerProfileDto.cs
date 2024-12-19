using MixBalancer.Application.Dtos.Match;

namespace MixBalancer.Application.Dtos.Player
{
    public class PlayerProfileDto
    {
        public string Nickname { get; set; }
        public int SkillLevel { get; set; }
        public double WinRate { get; set; }
        public double KDRatio { get; set; }
        public double HeadshotPercentage { get; set; }
        public int TotalMatches { get; set; }
        public string BestMap { get; set; }
        public string WorstMap { get; set; }
        public int Aces { get; set; }
        public int Clutches { get; set; }
        public List<MatchHistoryDto> MatchHistory { get; set; }
    }
}