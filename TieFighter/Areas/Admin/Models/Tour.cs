using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TieFighter.Models
{
    public class Tour
    {
        [Key]
        public string TourId { get; set; }
        public string TourName { get; set; }
        public int Position { get; set; }

        [NotMapped]
        public IList<Mission> Missions { get; set; }
    }
}
