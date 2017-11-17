using System;
using System.Collections.Generic;

namespace TieFighter.Data.Migrations
{
    public partial class Missions
    {
        public int MissionId { get; set; }
        public int? FkTour { get; set; }
        public string MissionName { get; set; }

        public Tours FkTourNavigation { get; set; }
    }
}
