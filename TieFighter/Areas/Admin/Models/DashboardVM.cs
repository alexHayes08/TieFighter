using System.Collections.Generic;
using TieFighter.Models;
using TieFighter.Models.HomeViewModels;

namespace TieFighter.Areas.Admin.Models
{
    public class DashboardVM
    {
        public IList<UserGameViewModel> LatestUsers { get; set; }
        public IList<Mission> MostPopularMissions { get; set; }
        public IList<Ship> MostPopularShips { get; set; }
    }
}
