using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Datastore.V1;
using Newtonsoft.Json.Linq;

namespace TieFighter.Models
{
    public class GameMode : IDatastoreEntityAndJsonBinding
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public IList<Stage> AllowedTypesOfStages { get; set; }
    }
}
