using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Google.Cloud.Datastore.V1;

namespace TieFighter.Models
{
    public enum ShipComponentType
    {
        LaserCannon,
        IonCannon,
        ProtonTorpedo,
        ConcussionMissile,
        LifeSupport,
        Engine,
        Thruster,
        FuelTank,
        SheildGenerator,
        DroidSocket,
        Cockpit,
        Sensors,
        CloakingDevice,
        HyperDrive
    };

    public class ShipComponent : IDatastoreEntityAndJsonBinding
    {
        [Required]
        public string DisplayName { get; set; }
        [NotMapped]
        public object Value { get; set; }
        public ShipComponentType? Type { get; set; }

        public override IDatastoreEntityAndJsonBinding FromEntity(Entity entity)
        {
            var shipComponent = new ShipComponent()
            {
                Id = entity.Key.ToId(),
                DisplayName = entity[nameof(DisplayName)]?.StringValue,
            };

            var typeNameString = entity[nameof(Type)]?.StringValue;
            if (typeNameString != null)
            {
                shipComponent.Type = (ShipComponentType)Enum.Parse(typeof(ShipComponentType), typeNameString);
            }

            return shipComponent;
        }

        public override Key GenerateNewKey(DatastoreDb db, params IDatastoreEntityAndJsonBinding[] ancestors)
        {
            var incompleteKey = db.CreateKeyFactory(nameof(ShipComponent)).CreateIncompleteKey();
            return db.Insert(new Entity()
            {
                Key = incompleteKey
            });
        }

        public override Entity ToEntity()
        {
            var e = new Entity()
            {
                Key = Startup.DatastoreDb.ShipComponentsKeyFactory.CreateKey(Id.Value),
                [nameof(DisplayName)] = DisplayName,
            };

            if (Type.HasValue)
            {
                e[nameof(Type)] = Enum.GetName(typeof(ShipComponentType), Type);
            }

            return e;
        }
    }
}
