using Google.Cloud.Datastore.V1;
using Newtonsoft.Json.Linq;

namespace TieFighter.Models
{
    public class ThreeDimensionsCoord : IDatastoreEntityAndJsonBinding
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public override void FromEntity(Entity entity)
        {
            X = entity[nameof(X)].DoubleValue;
            Y = entity[nameof(Y)].DoubleValue;
            Z = entity[nameof(Z)].DoubleValue;
        }

        public override void FromJObject(JObject json)
        {
            if (double.TryParse(json[nameof(X)].ToString(), out double newX))
            {
                X = newX;
            }

            if (double.TryParse(json[nameof(Y)].ToString(), out double newY))
            {
                Y = newY;
            }

            if (double.TryParse(json[nameof(Z)].ToString(), out double newZ))
            {
                Z = newZ;
            }
        }

        public override Entity ToEntity()
        {
            return new Entity()
            {
                [nameof(X)] = X,
                [nameof(Y)] = Y,
                [nameof(Z)] = Z
            };
        }

        public override JObject ToJObject()
        {
            return JObject.FromObject(this);
        }
    }
}
