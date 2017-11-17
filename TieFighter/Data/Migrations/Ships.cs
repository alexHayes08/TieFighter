using System;
using System.Collections.Generic;

namespace TieFighter.Data.Migrations
{
    public partial class Ships
    {
        public int ShipId { get; set; }
        public string ShipFolder { get; set; }
        public string ShipName { get; set; }
    }
}
