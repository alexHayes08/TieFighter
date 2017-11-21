using System.Collections.Generic;

namespace TieFighter.Models
{
    public class ApplicationUserDatastoreModel
    {
        public string Id { get; set; }
        public IList<Medal> MedalsWon { get; set; }
        public IList<CampaignTourStat> CampaignTourStats { get; set; }
        public IList<int> ShipsUnlocked { get; set; }
    }
}
