using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TieFighter.Models
{
    public class Mission
    {
        [Key]
        public long Id { get; set; }
        public string DisplayName { get; set; }
        public int PositionInTour { get; set; }
        public string MissionBriefing { get; set; }
        public DateTime LastPlayedOn { get; set; }
        public string TourId { get; set; }
        [NotMapped]
        public Stage[] Stages { get; set; }
    }
}
