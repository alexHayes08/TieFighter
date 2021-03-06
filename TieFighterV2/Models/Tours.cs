﻿using System;
using System.Collections.Generic;

namespace TieFighterV2.Models
{
    public partial class Tours
    {
        public Tours()
        {
            Missions = new HashSet<Missions>();
        }

        public int TourId { get; set; }
        public string TourName { get; set; }

        public ICollection<Missions> Missions { get; set; }
    }
}
