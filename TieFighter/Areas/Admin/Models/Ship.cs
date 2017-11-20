using System.ComponentModel.DataAnnotations;

namespace TieFighter.Models
{
    public class Ship
    {
        [Key]
        public int ID { get; set; }
        public string DisplayName { get; set; }
        public string Folder { get; set; }
    }
}
