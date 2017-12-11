using System;

namespace TieFighter.Models
{
    public class MissionStat
    {
        public string Id { get; set; }
        public string MissionName { get; set; }
        public int MissionId { get; set; }
        public bool FinishedMission { get; set; }
        public TimeSpan BestTime { get; set; }
        public double BestScore { get; set; }
    }
}
