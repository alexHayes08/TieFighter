using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TieFighter.Models
{
    public class Medal
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string MedalName { get; set; }
        [MaxLength(200)]
        public string Description { get; set; }
        [Range(0, double.MaxValue)]
        public double PointsWorth { get; set; }
        public string FileLocation { get; set; }
    }
}
