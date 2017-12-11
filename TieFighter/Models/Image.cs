using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Datastore.V1;
using Newtonsoft.Json.Linq;

namespace TieFighter.Models
{
    public class Image : IDatastoreEntityAndJsonBinding
    {
        [Key]
        public string Id { get; set; }
        public string FileName { get; set; }
        [ForiegnEntity(nameof(Game.Id))]
        public Game GameImage { get; set; }
        [ForiegnEntity(nameof(User.Id))]
        public User UserImage { get; set; }
    }
}
