using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TieFighter.Models;

namespace TieFighter.Models
{
    public class UsersToMedals
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Range(1, Double.MaxValue)]
        public int Quantity { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int MedalId { get; set; }
        public Medal Medal { get; set; }
    }
}