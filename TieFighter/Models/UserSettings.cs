using Google.Cloud.Datastore.V1;
using Newtonsoft.Json.Linq;

namespace TieFighter.Models
{
    public class UserSettings : IDatastoreEntityAndJsonBinding
    {
        public ControlSettings Controls { get; set; }
        public AudioSettings Audio { get; set; }
        public VideoSettings Video { get; set; }

        public override Entity ToEntity()
        {
            var entity = new Entity();
            entity[nameof(Audio)] = Audio.ToEntity();
            entity[nameof(Controls)] = Controls.ToEntity();
            entity[nameof(Video)] = Video.ToEntity();

            return entity;
        }
    }
}
