using System;
using System.Collections.Generic;
using Google.Cloud.Datastore.V1;
using Newtonsoft.Json.Linq;

namespace TieFighter.Models
{
    public class User : IDatastoreEntityAndJsonBinding
    {
        public string Id { get; set; }
        public IList<string> MedalsWon { get; set; }
        public IList<CampaignTourStat> CampaignTourStats { get; set; }
        public IList<string> ShipsUnlocked { get; set; }
        public UserSettings Settings { get; set; }

        public override Entity ToEntity(params IDatastoreEntityAndJsonBinding[] ancestors)
        {
            var entity = new Entity();
            entity.Key = Startup.DatastoreDb.UsersKeyFactory.CreateKey(Id);
            var medalsWon = new List<Entity>();
            foreach (var medalWon in MedalsWon)
            {
                medalsWon.Add(new Entity()
                {
                    [nameof(Medal.Id)] = medalWon
                });
            }
            var shipsUnlocked = new List<Entity>();
            foreach (var shipUnlocked in ShipsUnlocked)
            {
                shipsUnlocked.Add(new Entity()
                {
                    [nameof(Ship.Id)] = shipUnlocked
                });
            }
            entity[nameof(MedalsWon)] = medalsWon.ToArray();
            entity[nameof(CampaignTourStats)] = DatastoreHelpers.ObjectsToEntities(Startup.DatastoreDb, CampaignTourStats);
            entity[nameof(ShipsUnlocked)] = shipsUnlocked.ToArray();
            entity[nameof(Settings)] = Settings.ToEntity();

            return entity;
        }

        public override JObject ToJObject()
        {
            return JObject.FromObject(this);
        }

        public static explicit operator User(ApplicationUser user)
        {
            var u = new User();
            var key = Startup.DatastoreDb.UsersKeyFactory.CreateKey(user.Id);
            var entity = Startup.DatastoreDb.Db.Lookup(key);
            return DatastoreHelpers.ParseEntityToObject<User>(entity);
        }
    }
}
