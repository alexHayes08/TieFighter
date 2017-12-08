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

        public override void FromEntity(Entity entity)
        {
            throw new NotImplementedException();
        }

        public override void FromJObject(JObject json)
        {
            throw new NotImplementedException();
        }

        public override Entity ToEntity()
        {
            throw new NotImplementedException();
        }

        public override JObject ToJObject()
        {
            throw new NotImplementedException();
        }
    }
}
