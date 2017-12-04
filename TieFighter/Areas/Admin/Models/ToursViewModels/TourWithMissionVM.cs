using System.Collections.Generic;
using TieFighter.Models;

namespace TieFighter.Areas.Admin.Models.ToursViewModels
{
    public class TourWithMissionVM
    {
        public Tour Tour { get; set; }
        public IList<Tour> ToursWithConflictingPositions { get; set; }
    }
}
