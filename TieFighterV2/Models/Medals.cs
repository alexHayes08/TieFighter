using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TieFighterV2.Models
{
    public class Medals
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string MedalName { get; set; }
        [MaxLength(200)]
        public string Description { get; set; }
        [Range(0, double.MaxValue)]
        public int PointsWorth { get; set; }
    }
}
