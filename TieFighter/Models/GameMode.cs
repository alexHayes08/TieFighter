using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Datastore.V1;
using Newtonsoft.Json.Linq;

namespace TieFighter.Models
{
    public class GameMode : IDatastoreEntityAndJsonBinding
    {
        [Required]
        public string Name { get; set; }
        public bool IsEnabled { get; set; }
        [Display(Name = "Game Mode Category")]
        public long GameModeCategoryId { get; set; }
        [NotMapped]
        public IList<Stage> AllowedTypesOfStages { get; set; }
        [NotMapped]
        public IList<object> Properties { get; set; }

        public override IDatastoreEntityAndJsonBinding FromEntity(Entity entity)
        {
            return new GameMode()
            {
                Id = entity.Key.ToId(),
                Name = entity[nameof(Name)].StringValue,
                IsEnabled = entity[nameof(IsEnabled)].BooleanValue,
                GameModeCategoryId = entity[nameof(GameModeCategoryId)].IntegerValue
            };
        }

        public override Entity ToEntity()
        {
            return new Entity()
            {
                Key = DatastoreDbReference.GameModesFactory.CreateKey(Id.Value),
                [nameof(IsEnabled)] = IsEnabled,
                [nameof(Name)] = Name,
                [nameof(GameModeCategoryId)] = GameModeCategoryId
            };
        }

        public override Key GenerateNewKey(DatastoreDb db, params IDatastoreEntityAndJsonBinding[] ancestors)
        {
            var incompleteKey = db.CreateKeyFactory(nameof(GameMode)).CreateIncompleteKey();
            return db.Insert(new Entity()
            {
                Key = incompleteKey
            });
        }
    }
}
