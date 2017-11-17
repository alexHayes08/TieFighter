using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TieFighter.Models
{
    public class Tour
    {
        [Key]
        public int TourID { get; set; }
        public string TourName { get; set; }

        ICollection<Mission> Missions { get; set; }
    }
}
