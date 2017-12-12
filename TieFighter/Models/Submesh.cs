using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Google.Cloud.Datastore.V1;
using Newtonsoft.Json.Linq;

namespace TieFighter.Models
{

    public class Submesh : IDatastoreEntityAndJsonBinding
    {
        public string MeshName { get; set; }
        public long ShipId { get; set; }
        [NotMapped]
        public ShipComponent[] Components { get; set; }
        public ThreeDimensionsCoord TranslationOffset { get; set; }
        public ThreeDimensionsCoord ScaleOffset { get; set; }
        public ThreeDimensionsCoord RotationOffset { get; set; }

        private void CopyPropertiesOf(Submesh other)
        {
            if (other.Id.HasValue)
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

        public override IDatastoreEntityAndJsonBinding FromEntity(Entity entity)
        {
            var newMesh = DatastoreHelpers.ParseEntityToObject<Submesh>(entity);
            //Id = newMesh.Id;
            //Components = newMesh.Components;
            //TranslationOffset = newMesh.TranslationOffset;
            //RotationOffset = newMesh.RotationOffset;
            //ScaleOffset = newMesh.ScaleOffset;
            return newMesh;
        }
    }
}
