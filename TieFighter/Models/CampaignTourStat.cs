using System.Collections.Generic;

namespace TieFighter.Models
{
    public class CampaignTourStat
    {
        public int TourId { get; set; }
        public string TourName { get; set; }
        public IList<MissionStat> Missions { get; set; }
    }
}
