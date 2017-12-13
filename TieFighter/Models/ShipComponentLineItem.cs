using Google.Cloud.Datastore.V1;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TieFighter.Models
{
    public class ShipComponentLineItem : IDatastoreEntityAndJsonBinding
    {
        [Required]
        public long SubmeshId { get; set; }
        [Required]
        public long ShipCompnentId { get; set; }
        [Required]
        [Range(1,1000)]
        public int Quantity { get; set; }
        [NotMapped]
        public ShipComponent ShipCompnent { get; set; }

        public override Entity ToEntity()
        {
            return DatastoreHelpers.ObjectToEntity(Startup.DatastoreDb, this);
        }

        public override IDatastoreEntityAndJsonBinding FromEntity(Entity entity)
        {
            return DatastoreHelpers.ParseEntityToObject<ShipComponentLineItem>(entity);
        }
    }
}
