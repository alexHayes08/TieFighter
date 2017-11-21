using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TieFighter.Models
{
    public class Mission
    {
        [Key]
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public int PositionInTour { get; set; }
        public string MissionBriefing { get; set; }
        [ForeignKey("TourID")]
        public Tour Tour { get; set; }
    }
}
