using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Datastore.V1;
using Microsoft.AspNetCore.Http;

namespace TieFighter.Models
{
    public class Ship : IDatastoreEntityAndJsonBinding
    {
        public string DisplayName { get; set; }
        [NotMapped]
        public Submesh[] Submeshes { get; set; }

        public override IDatastoreEntityAndJsonBinding FromEntity(Entity entity)
        {
            var ship = new Ship()
            {
                Id = entity.Key.ToId(),
                DisplayName = entity[nameof(DisplayName)].StringValue,
            };

            return ship;
        }

        public override Entity ToEntity()
        {
            var key = Startup.DatastoreDb.ShipsKeyFactory.CreateIncompleteKey();

            return new Entity()
            {
                Key = Startup.DatastoreDb.Db.Insert(new Entity { Key = key }),
                [nameof(DisplayName)] = DisplayName,
            };
        }

        public override Key GenerateNewKey(DatastoreDb db, params IDatastoreEntityAndJsonBinding[] ancestors)
        {
            var key = db.CreateKeyFactory(nameof(Ship)).CreateIncompleteKey();
            return db.Insert(new Entity { Key = key });
        }

        public string GetFilePath ()
        {
            return Path.Combine("wwwroot", "resources", "Models", $"{Id}.babylon");
        }

        public async void UpdateFileAsync(IFormFileCollection files)
        {
            if (files.Count > 0)
            {
                // Use only the first one
                try
                {
                    using (var fileStream = new FileStream(GetFilePath(), FileMode.Create))
                    {
                        await files[0].CopyToAsync(fileStream);
                    }
                }
                catch
                { }
            }
        }

        public void SetSubmeshes()
        {
            var submeshesQuery = new Query(nameof(Submesh))
            {
                Filter = Filter.Equal(nameof(Submesh.ShipId), Id)
            };

            var submeshEntities = Startup.DatastoreDb.Db.RunQuery(submeshesQuery).Entities;
            var submeshes = DatastoreHelpers.ParseEntitiesToObject<Submesh>(submeshEntities);

            foreach (var submesh in submeshes)
            {
                var componentsQuery = new Query(nameof(ShipComponentLineItem))
                {
                    Filter = Filter.Equal(nameof(ShipComponentLineItem.SubmeshId), submesh.Id)
                };
                var components = DatastoreHelpers.ParseEntitiesToObject<ShipComponentLineItem>(
                    Startup.DatastoreDb.Db.RunQuery(componentsQuery).Entities
                );

                foreach (var component in components)
                {
                    var shipComponent = DatastoreHelpers.ParseEntityToObject<ShipComponent>(
                        Startup.DatastoreDb.Db.Lookup(
                            Startup.DatastoreDb.ShipComponentsKeyFactory.CreateKey(component.ShipCompnentId)
                        )
                    );

                    component.ShipCompnent = shipComponent;
                }
            }

            Submeshes = submeshes.ToArray();
        }

        public void DeleteSubmeshes()
        {
            var submeshesQuery = new Query(nameof(Submesh))
            {
                Filter = Filter.Equal(nameof(Submesh.ShipId), Id)
            };

            var submeshEntities = Startup.DatastoreDb.Db.RunQuery(submeshesQuery).Entities;
            var submeshes = DatastoreHelpers.ParseEntitiesToObject<Submesh>(submeshEntities);

            foreach (var submesh in submeshes)
            {
                var componentsQuery = new Query(nameof(ShipComponentLineItem))
                {
                    Filter = Filter.Equal(nameof(ShipComponentLineItem.SubmeshId), submesh.Id)
                };
                var components = DatastoreHelpers.ParseEntitiesToObject<ShipComponentLineItem>(
                    Startup.DatastoreDb.Db.RunQuery(componentsQuery).Entities
                );

                foreach (var component in components)
                {
                    Startup.DatastoreDb.Db.Delete(
                        Startup.DatastoreDb.Db.Lookup(
                            Startup.DatastoreDb.ShipComponentsKeyFactory.CreateKey(component.ShipCompnentId)
                        )
                    );
                }
            }

            Startup.DatastoreDb.Db.Delete(submeshEntities);
            Submeshes = new Submesh[0];
        }
    }
}
