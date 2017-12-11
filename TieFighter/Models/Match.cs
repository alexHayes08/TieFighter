using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Datastore.V1;
using Newtonsoft.Json.Linq;

namespace TieFighter.Models
{
    public class Match : IDatastoreEntityAndJsonBinding
    {
        [Key]
        public string Id { get; set; }
        [ForiegnEntityAttribute(nameof(GameMode.Id))]
        public Game Game { get; set; }
        public GameMode Mode { get; set; }
        public User[] Players { get; set; }
        public bool EnableMedals { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
