using System.Collections.Generic;

namespace TieFighter.Models
{
    public class User
    {
        public string Id { get; set; }
        public IList<Medal> MedalsWon { get; set; }
        public IList<CampaignTourStat> CampaignTourStats { get; set; }
        public IList<string> ShipsUnlocked { get; set; }
        public UserSettings Settings { get; set; }
    }
}
