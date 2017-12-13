using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Google.Cloud.Datastore.V1;

namespace TieFighter.Models
{
    public class Medal : IDatastoreEntityAndJsonBinding
    {
        [Required]
        public string MedalName { get; set; }
        [MaxLength(200)]
        public string Description { get; set; }
        [Range(0, double.MaxValue)]
        public double PointsWorth { get; set; }
        public MedalCondition[] Conditions { get; set; }

        public override IDatastoreEntityAndJsonBinding FromEntity(Entity entity)
        {
            var medal = new Medal()
            {
                Id = entity.Key.ToId(),
                Description = entity[nameof(Medal.Description)].StringValue,
                MedalName = entity[nameof(Medal.MedalName)].StringValue,
                PointsWorth = entity[nameof(Medal.PointsWorth)].DoubleValue
            };

            return medal;
        }

        public override Entity ToEntity()
        {
            var entity = new Entity()
            {
                [nameof(MedalName)] = MedalName,
                [nameof(Description)] = Description,
                [nameof(PointsWorth)] = PointsWorth
            };

            if (Id != null)
            {
                entity.Key = Startup.DatastoreDb.GamesKeyFactory.CreateKey(Id.Value);
            }

            return entity;
        }

        public override Key GenerateNewKey(DatastoreDb db, params IDatastoreEntityAndJsonBinding[] ancestors)
        {
            var incompleteKey = db.CreateKeyFactory(nameof(Medal)).CreateIncompleteKey();
            return db.Insert(new Entity()
            {
                Key = incompleteKey
            });
        }
    }
}
