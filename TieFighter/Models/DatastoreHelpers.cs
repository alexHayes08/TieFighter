using Google.Cloud.Datastore.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace TieFighter.Models
{
    public static class DatastoreHelpers
    {
        public static T ParseEntityToObject<T>(Entity entity) where T:new()
        {
            T newInstance = (T)Activator.CreateInstance(typeof(T));
            var castedType = newInstance as IDatastoreEntityAndJsonBinding;
            if (castedType != null)
            {
                // If the type implements IDatastoreEntityAndJsonBinding, use it
                castedType.FromEntity(entity);
                return newInstance;
            }
            else
            {
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
                            var blah = entity.Properties[property.Name].ValueTypeCase; ;
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
                    catch (Exception e)
                    { }
                }

                return newInstance;
            }
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

        public static Entity ObjectToEntity<T>(TieFighterDatastoreContext db, T obj, string idPropertyName = null, params string[] indexedProperties) where T:new()
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

            bool isIdAlreadySet = false;

            // Setup the entity's id
            if (!string.IsNullOrEmpty(idPropertyName))
            {
                string idValue = objType.GetProperty(idPropertyName).GetValue(obj).ToString();
                entity.Key = db.GetKeyFactoryForKind(objType.Name).CreateKey(idValue);
                isIdAlreadySet = true;
            }

            foreach (var prop in objProperties)
            {
                // Ignore this property if it's the id and the id has already been set
                if (isIdAlreadySet 
                    && (prop.IsDefined(typeof(KeyAttribute))) || prop.Name == idPropertyName)
                {
                    continue;
                }

                var propertyType = prop.PropertyType;

                if (prop.IsDefined(typeof(NotMappedAttribute)))
                {

                    // Ignore this property if it contains the NotMapped Attribute
                    continue;
                }
                else if (prop.IsDefined(typeof(KeyAttribute)) && !isIdAlreadySet)
                {
                    string idValue = prop.GetValue(obj).ToString();
                    entity.Key = db.GetKeyFactoryForKind(objType.Name).CreateKey(idValue);
                }
                else if (propertyType == typeof(Enum))
                {
                    entity[prop.Name] = Enum.GetName(propertyType, objType.GetProperty(prop.Name).GetValue(obj));
                }
                else if (propertyType == typeof(string))
                {
                    entity[prop.Name] = objType.GetProperty(prop.Name).GetValue(obj) as string;
                }
                else if (propertyType == typeof(double))
                {
                    entity[prop.Name] = objType.GetProperty(prop.Name).GetValue(obj) as double?;
                }
                else if (propertyType == typeof(int))
                {
                    entity[prop.Name] = objType.GetProperty(prop.Name).GetValue(obj) as int?;
                }
                else if (propertyType == typeof(DateTime))
                {
                    entity[prop.Name] = objType.GetProperty(prop.Name).GetValue(obj) as DateTime?;
                }
                else if (propertyType == typeof(bool))
                {
                    entity[prop.Name] = objType.GetProperty(prop.Name).GetValue(obj) as bool?;
                }
                else if (propertyType.IsArray)
                {
                    Type elType = propertyType.GetElementType();
                    if (objType.GetProperty(prop.Name).GetValue(obj) is object[] objArr)
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

        public static Entity[] ObjectsToEntities<T>(TieFighterDatastoreContext db, IEnumerable<T> objects) where T:new()
        {
            var entities = new List<Entity>();
            foreach (var obj in objects)
                entities.Add(ObjectToEntity(db, obj));

            return entities.ToArray();
        }

        public static string GetEntityKey(this Entity entity)
        {
            return entity.Key.Path[0].Name;
        }

        public static T GetEntityProperty<T>(this Entity entity, string propertyName) where T:class
        {
            return entity[propertyName] as T;
        }

        public static long ToId (this Key key)
        {
            var blah = key.Path;
            return blah[blah.Count - 1].Id;
        }
    }
}
