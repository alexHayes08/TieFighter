using Google.Cloud.Datastore.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace TieFighter.Models
{
    public static class DatastoreHelpers
    {
        public static T ParseEntityToObject<T>(Entity entity) where T:new()
        {
            T newInstance = (T)Activator.CreateInstance(typeof(T));

            foreach (var property in newInstance.GetType().GetProperties())
            {
                // Ignore property if NotMapped
                if (property.IsDefined(typeof(NotMappedAttribute), false))
                {
                    continue;
                }

                // Check if the property is part of the key
                if (property.Name == "Id" || property.IsDefined(typeof(KeyAttribute), false))
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
                                property.SetValue(newInstance, Convert.ToInt32(entity[property.Name].IntegerValue));
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
                                property.SetValue(newInstance, entity[property.Name].TimestampValue.ToDateTime());
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

        public static Entity ObjectToEntity<T>(TieFighterDatastoreContext db, T obj, string idPropertyName, params string[] indexedProperties) where T:new()
        {
            if (db == null)
            {
                throw new ArgumentNullException($"The argument '{nameof(db)}' cannot be null.");
            }
            //else if (string.IsNullOrEmpty(idPropertyName))
            //{
            //    throw new ArgumentException($"The argument '{nameof(idPropertyName)}' cannot be null nor emtpy.");
            //}

            var entity = new Entity();
            var objType = obj.GetType();
            var objProperties = objType.GetProperties();

            // Setup the entity's id
            if (!string.IsNullOrEmpty(idPropertyName))
            {
                string idValue = objType.GetProperty(idPropertyName).GetValue(obj).ToString();
                entity.Key = db.GetKeyFactoryForKind(objType.Name).CreateKey(idValue);
            }

            foreach (var prop in objProperties)
            {
                // Ignore the id
                if (prop.Name == idPropertyName)
                    continue;

                var propertyType = prop.PropertyType;
                if (propertyType == typeof(string))
                {
                    entity[prop.Name]= objType.GetProperty(prop.Name).GetValue(obj) as string;
                }
                else if (propertyType == typeof(double))
                {
                    entity[prop.Name]= objType.GetProperty(prop.Name).GetValue(obj) as double?;
                }
                else if (propertyType == typeof(int))
                {
                    entity[prop.Name]= objType.GetProperty(prop.Name).GetValue(obj) as int?;
                }
                else if (propertyType == typeof(DateTime))
                {
                    entity[prop.Name]= objType.GetProperty(prop.Name).GetValue(obj) as DateTime?;
                }
                else if (propertyType == typeof(bool))
                {
                    entity[prop.Name] = objType.GetProperty(prop.Name).GetValue(obj) as bool?;
                }
                else if (propertyType.IsArray)
                {
                    var objArr = objType.GetProperty(prop.Name).GetValue(obj) as object[];
                    Type elType = propertyType.GetElementType();
                    if (objArr != null)
                    {
                        var entities = new List<Entity>();
                        foreach (var element in objArr)
                        {
                            var el = Convert.ChangeType(element, elType);
                            entities.Add(ObjectToEntity(db, el, null));
                        }
                        entity[prop.Name] = entities.ToArray();
                    }
                    //throw new NotImplementedException("Handling arrays isn't supported yet.");
                }
                else
                {
                    throw new NotImplementedException($"Handling '${propertyType}' isn't yet supported.");
                }

                // Exclude from indexing if it's name isn't included in the indexedPropertyes array
                if (entity[prop.Name] != null)
                {
                     entity[prop.Name].ExcludeFromIndexes = !indexedProperties.Contains(prop.Name);
                }
            }

            return entity;
        }
    }
}
