using System;
using System.Collections.Generic;

namespace TieFighterV2.Models
{
    public partial class Missions
    {
        public int MissionId { get; set; }
        public int? FkTour { get; set; }
        public string MissionName { get; set; }

        public Tours FkTourNavigation { get; set; }
    }
}
