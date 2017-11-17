using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TieFighter.Models;

namespace TieFighterV2.Models
{
    public class UsersToMedals
    {
        [Range(1, Double.MaxValue)]
        public int Quantity { get; set; }

        public ApplicationUser User { get; set; }
        public Medal Medal { get; set; }
    }
}
