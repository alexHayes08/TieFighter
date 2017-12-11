using System;
using Google.Cloud.Datastore.V1;
using Newtonsoft.Json.Linq;

namespace TieFighter.Models
{
    public class AudioSettings : IDatastoreEntityAndJsonBinding
    {
        private string UserId { get; set; }
        public Volume MasterVolume { get; set; }
        public Volume MusicVolume { get; set; }
        public Volume CombatVolume { get; set; }
    }
}
