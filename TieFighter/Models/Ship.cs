using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TieFighter.Models
{
    public class Ship
    {
        [Key]
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string FileLocation { get; set; }
        [NotMapped]
        public Submesh[] Submeshes { get; set; }
    }
}
