using Google.Cloud.Datastore.V1;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace TieFighter.Models
{
    [Bind(include: nameof(Id) + ", " + nameof(Name) + ", " + nameof(IsEnabled))]
    public class Game : IDatastoreEntityAndJsonBinding
    {
        [Required]
        public string Name { get; set; }
        [Display(Name = "Is Enabled")]
        public bool IsEnabled { get; set; }
        public string GameUrl { get; set; }
        [NotMapped]
        public IList<string> ImageUrl { get; set; }
        [NotMapped]
        public IList<GameMode> GameModes { get; set; }

        public override IDatastoreEntityAndJsonBinding FromEntity(Entity entity)
        {
            var game = new Game
            {
                Id = entity.Key.ToId(),
                IsEnabled = entity[nameof(IsEnabled)]?.BooleanValue ?? false,
                Name = entity[nameof(Name)]?.StringValue,
                GameUrl = entity[nameof(GameUrl)]?.StringValue
            };

            return game;
        }

        public override Key GenerateNewKey(DatastoreDb db, params IDatastoreEntityAndJsonBinding[] ancestors)
        {
            var key = db.CreateKeyFactory(nameof(Key)).CreateIncompleteKey();
            return db.Insert(new Entity()
            {
                Key = key
            });

        }

        public override Entity ToEntity()
        {
            if (!Id.HasValue)
            {
                throw new System.Exception("Must have Id set before calling to Entity!");
            }

            return new Entity()
            {
                Key = Startup.DatastoreDb.GamesKeyFactory.CreateKey(Id.Value),
                [nameof(Name)] = Name,
                [nameof(IsEnabled)] = IsEnabled,
                [nameof(GameUrl)] = GameUrl
            };
        }
    }
}
