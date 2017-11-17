using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TieFighter.Models
{
    public class Mission
    {
        [Key]
        public int ID { get; set; }
        public string DisplayName { get; set; }
        [ForeignKey("TourID")]
        public Tour Tour { get; set; }
    }
}
