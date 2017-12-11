using System;
using Google.Cloud.Datastore.V1;
using Newtonsoft.Json.Linq;
using TieFighter.Models.Settings;

namespace TieFighter.Models
{
    public class VideoSettings : IDatastoreEntityAndJsonBinding
    {
        private string UserId { get; set; }
        public Resolution Resolution { get; set; }
        public ModelQuality ModelQuality { get; set; }
        public double ViewDistance { get; set; }
    }
}
