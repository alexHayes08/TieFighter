using Google.Cloud.Datastore.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TieFighter.Models
{
    public static class DatastoreHelpers
    {
        public static T ParseEntityToObject<T>(Entity entity) where T:new()
        {
            var newInstance = default(T);
            if (newInstance == null)
                newInstance = (T)Activator.CreateInstance(typeof(T));
            foreach (var property in newInstance.GetType().GetProperties())
            {
                // Check if the property is part of the key
                if (property.Name == "Id")
                {
                    property.SetValue(newInstance, entity.Key.Path[0].Name);
                    continue;
                }

                // Check if the property is part of the properties
                try
                {
                    if (entity.Properties[property.Name] != null)
                    {
                        var propertyType = property.PropertyType;
                        var blah = entity.Properties[property.Name].ValueTypeCase;;
                        switch (blah)
                        {
                            case Value.ValueTypeOneofCase.ArrayValue:
                                property.SetValue(newInstance, entity[property.Name].ArrayValue);
                                break;
                            case Value.ValueTypeOneofCase.BlobValue:
                                property.SetValue(newInstance, entity[property.Name].BlobValue);
                                break;
                            case Value.ValueTypeOneofCase.BooleanValue:
                                property.SetValue(newInstance, entity[property.Name].BooleanValue);
                                break;
                            case Value.ValueTypeOneofCase.DoubleValue:
                                property.SetValue(newInstance, entity[property.Name].DoubleValue);
                                break;
                            case Value.ValueTypeOneofCase.EntityValue:
                                property.SetValue(newInstance, entity[property.Name].EntityValue);
                                break;
                            case Value.ValueTypeOneofCase.GeoPointValue:
                                property.SetValue(newInstance, entity[property.Name].GeoPointValue);
                                break;
                            case Value.ValueTypeOneofCase.IntegerValue:
                                property.SetValue(newInstance, entity[property.Name].IntegerValue);
                                break;
                            case Value.ValueTypeOneofCase.KeyValue:
                                property.SetValue(newInstance, entity[property.Name].KeyValue);
                                break;
                            case Value.ValueTypeOneofCase.None:
                                property.SetValue(newInstance, entity[property.Name].NullValue);
                                break;
                            case Value.ValueTypeOneofCase.NullValue:
                                property.SetValue(newInstance, null);
                                break;
                            case Value.ValueTypeOneofCase.StringValue:
                                property.SetValue(newInstance, entity[property.Name].StringValue);
                                break;
                            case Value.ValueTypeOneofCase.TimestampValue:
                                property.SetValue(newInstance, entity[property.Name].TimestampValue);
                                break;
                        }
                    }
                }
                catch(Exception e)
                { }
            }

            return newInstance;
        }

        public static IList<T> ParseEntitiesToObject<T>(IEnumerable<Entity> entities) where T:new()
        {
            var objects = new List<T>();
            foreach (var entity in entities)
            {
                objects.Add(ParseEntityToObject<T>(entity));
            }

            return objects;
        }
    }
}
