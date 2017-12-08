using System;
using Google.Cloud.Datastore.V1;
using Newtonsoft.Json.Linq;
using TieFighter.Models.Settings;

namespace TieFighter.Models
{
    public class VideoSettings : IDatastoreEntityAndJsonBinding
    {
        private string UserId { get; set; }
        public Resolution Resulution { get; set; }
        public ModelQuality ModelQuality { get; set; }
        public double ViewDistance { get; set; }

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
