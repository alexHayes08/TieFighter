using System.ComponentModel.DataAnnotations;

namespace TieFighter.Models
{
    public class Ship
    {
        [Key]
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public string FileLocation { get; set; }
    }
}
