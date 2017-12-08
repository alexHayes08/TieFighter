using Google.Cloud.Datastore.V1;
using Newtonsoft.Json.Linq;

namespace TieFighter.Models
{
    public abstract class IDatastoreEntityAndJsonBinding
    {
        public abstract Entity ToEntity();
        public abstract void FromEntity(Entity entity);
        public abstract JObject ToJObject();
        public abstract void FromJObject(JObject json);
    }
}
