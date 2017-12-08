using System.ComponentModel.DataAnnotations;
using Google.Cloud.Datastore.V1;
using Newtonsoft.Json.Linq;

namespace TieFighter.Models
{

    public class Submesh : IDatastoreEntityAndJsonBinding
    {
        [Key]
        public string Id { get; set; }
        public ShipComponent[] Components { get; set; }
        public ThreeDimensionsCoord TranslationOffset { get; set; }
        public ThreeDimensionsCoord ScaleOffset { get; set; }
        public ThreeDimensionsCoord RotationOffset { get; set; }

        private void CopyPropertiesOf(Submesh other)
        {
            if (!string.IsNullOrEmpty(other.Id))
                Id = other.Id;

            if (other.Components != null)
                Components = other.Components;

            if (other.TranslationOffset != null)
                TranslationOffset = other.TranslationOffset;

            if (other.RotationOffset != null)
                RotationOffset = other.RotationOffset;

            if (other.ScaleOffset != null)
                ScaleOffset = other.ScaleOffset;
        }

        public override void FromEntity(Entity entity)
        {
            var newMesh = DatastoreHelpers.ParseEntityToObject<Submesh>(entity);
            Id = newMesh.Id;
            Components = newMesh.Components;
            TranslationOffset = newMesh.TranslationOffset;
            RotationOffset = newMesh.RotationOffset;
            ScaleOffset = newMesh.ScaleOffset;
        }

        public override void FromJObject(JObject json)
        {
            var newMesh = json.ToObject<Submesh>();
            
        }

        public override Entity ToEntity()
        {
            return DatastoreHelpers.ObjectToEntity(Startup.DatastoreDb, this);
        }

        public override JObject ToJObject()
        {
            return JObject.FromObject(this);
        }
    }
}
