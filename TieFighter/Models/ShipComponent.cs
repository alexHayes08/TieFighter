using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Google.Cloud.Datastore.V1;

namespace TieFighter.Models
{
    public enum ShipComponentType
    {
        Other = 0,
        LaserCannon = 1,
        IonCannon = 2,
        ProtonTorpedo = 3,
        ConcussionMissile = 4,
        LifeSupport = 5,
        Engine = 6,
        Thruster = 7,
        FuelTank = 8,
        SheildGenerator = 9,
        DroidSocket = 10,
        Cockpit = 11,
        Sensors = 12,
        CloakingDevice = 13,
        HyperDrive = 14
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
