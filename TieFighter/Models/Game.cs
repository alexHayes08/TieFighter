using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TieFighter.Models
{
    [Bind(include: nameof(Id) + ", " + nameof(Name) + ", " + nameof(IsEnabled))]
    public class Game : IDatastoreEntityAndJsonBinding
    {
        [Required]
        public string Name { get; set; }
        [Display(Name = "Is Enabled")]
        public bool IsEnabled { get; set; }
        [NotMapped]
        public IList<string> ImageUrl { get; set; }
        [NotMapped]
        public IList<GameMode> GameModes { get; set; }
    }
}
