using Google.Cloud.Datastore.V1;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;

namespace TieFighter.Models
{
    public abstract class IDatastoreEntityAndJsonBinding
    {
        #region Constructors

        public IDatastoreEntityAndJsonBinding()
        { }

        public IDatastoreEntityAndJsonBinding(bool createNewId = false)
        {
            if (createNewId)
            {
                Id = GenerateNewKey(DatastoreDbReference.Db).ToId();
            }
        }

        #endregion

        [Key]
        public long? Id { get; set; }

        public virtual JObject ToJObject()
        {
            return JObject.FromObject(this);
        }

        public virtual IDatastoreEntityAndJsonBinding FromJObject(JObject json)
        {
            var type = GetType();
            return json.ToObject(type) as IDatastoreEntityAndJsonBinding;
            //var localProperties = type.GetProperties();
            //var localPropertiesNames = localProperties.Select(p => p.Name).ToList();

            //foreach (var propertyName in localPropertiesNames)
            //{
            //    if (json[propertyName] != null)
            //    {
            //        var p = localProperties.Where(lp => lp.Name == propertyName).First();
            //        p.SetValue(this, json[propertyName]);
            //    }
            //}
        }

        public virtual Key GenerateNewKey(DatastoreDb db, params IDatastoreEntityAndJsonBinding[] ancestors)
        {
            var type = GetType();
            var ancestorTypes = GetAncestorTypes();
            if (ancestors.Length != ancestorTypes.Count)
            {
                throw new ArgumentOutOfRangeException();
            }
            Key key = null;

            // Skip the 'first' key
            for (var i = ancestorTypes.Count - 2; i >= 0; i--)
            {
                if (key == null)
                    key = db.CreateKeyFactory(ancestorTypes[i].Name).CreateKey(ancestors[i].Id.Value);
                else
                    key = new KeyFactory(key, ancestorTypes[i].Name).CreateKey(ancestors[i].Id.Value);
            }

            var entity = new Entity();

            // Null check again
            if (key == null)
            {
                entity.Key = db.CreateKeyFactory(type.Name).CreateIncompleteKey();
            }

            key = db.Insert(entity);

            return key;
        }

        public Key ToKey(DatastoreDb db, params IDatastoreEntityAndJsonBinding[] ancestors)
        {
            var ancestorTypes = GetAncestorTypes();

            if (ancestors.Length != ancestorTypes.Count)
            {
                throw new ArgumentOutOfRangeException();
            }
            Key key = null;

            // Need to start at last
            for (var i = ancestorTypes.Count - 1; i >= 0; i--)
            {
                if (key == null)
                    key = db.CreateKeyFactory(ancestorTypes[i].Name).CreateKey(ancestors[i].Id.Value);
                else
                    key = new KeyFactory(key, ancestorTypes[i].Name).CreateKey(ancestors[i].Id.Value);
            }

            return key;
        }

        /// <summary>
        /// Used to retrieve all types that have the AncestorPathAttribute
        /// </summary>
        /// <returns>
        /// The ancestors are ordered from youngest to oldest
        /// </returns>
        private IList<Type> GetAncestorTypes()
        {
            var types = new List<Type>();
            var type = GetType();
            var currentType = type;
            while (currentType != null
                && currentType.IsDefined(typeof(AncestorPathAttribute)))
            {
                types.Add(currentType);
                currentType = GetAncestorType(currentType);
            }

            return types.ToArray();
        }

        /// <summary>
        /// Used to retrieve the direct ancestor type if it has the AncestorPathAttribute
        /// </summary>
        /// <param name="t"></param>
        /// <returns>
        /// Will return null if the type passed in lacks the AncestorPathAttribute
        /// </returns>
        private Type GetAncestorType(Type t)
        {
            if (t.IsDefined(typeof(AncestorPathAttribute)))
            {
                var attr = t.GetCustomAttribute<AncestorPathAttribute>();
                return attr.Ancestor;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Creates and populates an object with the properties and values 
        /// from the entity provided
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>New object with the values from the entity</returns>
        public virtual IDatastoreEntityAndJsonBinding FromEntity(Entity entity)
        {
            var type = GetType();
            var defaultInstance = Activator.CreateInstance(type);
            var localProperties = type.GetProperties();
            var localPropertiesNames = localProperties.Select(p => p.Name).ToList();
            var entityProperties = entity.Properties.Keys;

            // Set the key first
            var keyProp = localProperties.Where(p => p.IsDefined(typeof(KeyAttribute))).ToList();
            if (keyProp.Count == 1)
            {
                var prop = keyProp[0];
                if (long.TryParse(entity.GetEntityKey(), out long id))
                {
                    prop.SetValue(defaultInstance, id);
                }
            }


            foreach (var entityProperty in entityProperties)
            {
                if (!localPropertiesNames.Contains(entityProperty))
                {
                    throw new Exception("The entity had a property not available on the model!");
                }

                var p = localProperties.Where(lp => lp.Name == entityProperty).First();
                var propertyType = p.PropertyType;
                //if (propertyType.GetConstructor(Type.EmptyTypes) != null)
                //{
                if (propertyType == typeof(String))
                {
                    p.SetValue(defaultInstance, entity[entityProperty].StringValue);
                }
                else if (propertyType == typeof(double))
                {
                    p.SetValue(defaultInstance, entity[entityProperty].DoubleValue);
                }
                else if (propertyType == typeof(int))
                {
                    p.SetValue(defaultInstance, entity[entityProperty].IntegerValue);
                }
                else if (propertyType == typeof(bool))
                {
                    p.SetValue(defaultInstance, entity[entityProperty].BooleanValue);
                }
                //}
            }

            return defaultInstance as IDatastoreEntityAndJsonBinding;
        }

        /// <summary>
        /// Converts this into an entity
        /// </summary>
        /// <param name="ancestors">
        /// A list of objects to used to create the ancestor path for this entities key.
        /// </param>
        /// <returns></returns>
        public virtual Entity ToEntity(params IDatastoreEntityAndJsonBinding[] ancestors)
        {
            return ToEntity(false, ancestors);
        }

        /// <summary>
        /// Converts this object an entity
        /// </summary>
        /// <param name="populateLists">
        /// When true, will try and create entities to populate all IEnumerable properties
        /// on the entity with their respective values
        /// </param>
        /// <param name="ancestors">
        /// A list of objects to used to create the ancestor path for this entities key.
        /// </param>
        /// <returns></returns>
        public virtual Entity ToEntity(bool populateLists = false, params IDatastoreEntityAndJsonBinding[] ancestors)
        {
            var entity = new Entity();

            var type = GetType();
            var properties = type.GetProperties();
            var foreignProperties = new List<PropertyInfo>();
            bool setKey = false;

            foreach (var property in properties)
            {
                var propertyType = property.GetType();
                var propertyAttributes = property.Attributes;
                var propertyInterfaces = new List<Type>(propertyType.GetInterfaces());

                // If property has the 'NotMapped' attribute ignore it
                if (propertyType.IsDefined(typeof(NotMappedAttribute)))
                {
                    continue;
                }

                // Check if property is enumerable and contains the attribute ForiegnEntities
                else if (propertyInterfaces.Contains(typeof(IEnumerable))
                    && propertyType.IsDefined(typeof(ForiegnEntityAttribute)))
                {
                    foreignProperties.Add(property);
                    continue;
                }

                // Check if the property has the Key attribute
                else if (propertyType.IsDefined(typeof(KeyAttribute)))
                {
                    if (setKey)
                    {
                        throw new Exception("A class may not have more than one key!");
                    }
                    else
                    {
                        setKey = true;
                        var id = type.GetProperty(property.Name).GetValue(this) as string;
                        entity.Key = ToKey(DatastoreDbReference.Db);
                    }
                }

                // If not a class that implements IDatastoreEntity try and set the value
                else if (!propertyInterfaces.Contains(typeof(IDatastoreEntityAndJsonBinding)))
                {
                    var propVal = type.GetProperty(propertyType.Name).GetValue(this);
                    if (propVal is string)
                    {
                        entity[property.Name] = propVal as string;
                    }
                    else if (propVal is int?)
                    {
                        entity[property.Name] = propVal as int?;
                    }
                    else if (propVal is bool?)
                    {
                        entity[property.Name] = propVal as bool?;
                    }
                    else if (propVal is double?)
                    {
                        entity[property.Name] = propVal as double?;
                    }
                    else if (propVal is string[])
                    {
                        var values = propVal as string[];
                        var arrValue = new ArrayValue();
                        foreach (var val in values)
                        {
                            arrValue.Values.Add(new Value()
                            {
                                StringValue = val
                            });
                        }

                        entity[property.Name] = arrValue;
                    }
                    else if (propVal is int[])
                    {
                        var values = propVal as int[];
                        var arrValue = new ArrayValue();
                        foreach (var val in values)
                        {
                            arrValue.Values.Add(new Value()
                            {
                                IntegerValue = val
                            });
                        }

                        entity[property.Name] = arrValue;
                    }
                    else if (propVal is bool[])
                    {
                        var values = propVal as bool[];
                        var arrValue = new ArrayValue();
                        foreach (var val in values)
                        {
                            arrValue.Values.Add(new Value()
                            {
                                BooleanValue = val
                            });
                        }

                        entity[property.Name] = arrValue;
                    }
                    else if (propVal is double[])
                    {
                        var values = propVal as double[];
                        var arrValue = new ArrayValue();
                        foreach (var val in values)
                        {
                            arrValue.Values.Add(new Value()
                            {
                                DoubleValue = val
                            });
                        }

                        entity[property.Name] = arrValue;
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }

                // If the property implements IDatastoreEntity then the built in 
                // functionality to populate the entity
                else if (propertyInterfaces.Contains(typeof(IDatastoreEntityAndJsonBinding)))
                {
                    var propVal = type.GetProperty(propertyType.Name).GetValue(this) as IDatastoreEntityAndJsonBinding;
                    entity[property.Name] = propVal.ToEntity();
                }
            }

            return entity;
        }

        public virtual Entity ToEntity()
        {
            var entity = new Entity();

            var type = GetType();
            var properties = type.GetProperties();
            var foreignProperties = new List<PropertyInfo>();
            bool setKey = false;

            foreach (var property in properties)
            {
                var propertyType = property.GetType();
                var propertyAttributes = property.Attributes;
                var propertyInterfaces = new List<Type>(propertyType.GetInterfaces());

                // If property has the 'NotMapped' attribute ignore it
                if (propertyType.IsDefined(typeof(NotMappedAttribute)))
                {
                    continue;
                }

                // Check if property is enumerable and contains the attribute ForiegnEntities
                else if (propertyInterfaces.Contains(typeof(IEnumerable))
                    && propertyType.IsDefined(typeof(ForiegnEntityAttribute)))
                {
                    foreignProperties.Add(property);
                    continue;
                }

                // Check if the property has the Key attribute
                else if (propertyType.IsDefined(typeof(KeyAttribute)))
                {
                    if (setKey)
                    {
                        throw new Exception("A class may not have more than one key!");
                    }
                    else
                    {
                        setKey = true;
                        var id = type.GetProperty(property.Name).GetValue(this) as string;
                        entity.Key = ToKey(DatastoreDbReference.Db);
                    }
                }

                // If not a class that implements IDatastoreEntity try and set the value
                else if (!propertyInterfaces.Contains(typeof(IDatastoreEntityAndJsonBinding)))
                {
                    var propVal = type.GetProperty(propertyType.Name).GetValue(this);
                    if (propVal is string)
                    {
                        entity[property.Name] = propVal as string;
                    }
                    else if (propVal is int?)
                    {
                        entity[property.Name] = propVal as int?;
                    }
                    else if (propVal is bool?)
                    {
                        entity[property.Name] = propVal as bool?;
                    }
                    else if (propVal is double?)
                    {
                        entity[property.Name] = propVal as double?;
                    }
                    else if (propVal is string[])
                    {
                        var values = propVal as string[];
                        var arrValue = new ArrayValue();
                        foreach (var val in values)
                        {
                            arrValue.Values.Add(new Value()
                            {
                                StringValue = val
                            });
                        }

                        entity[property.Name] = arrValue;
                    }
                    else if (propVal is int[])
                    {
                        var values = propVal as int[];
                        var arrValue = new ArrayValue();
                        foreach (var val in values)
                        {
                            arrValue.Values.Add(new Value()
                            {
                                IntegerValue = val
                            });
                        }

                        entity[property.Name] = arrValue;
                    }
                    else if (propVal is bool[])
                    {
                        var values = propVal as bool[];
                        var arrValue = new ArrayValue();
                        foreach (var val in values)
                        {
                            arrValue.Values.Add(new Value()
                            {
                                BooleanValue = val
                            });
                        }

                        entity[property.Name] = arrValue;
                    }
                    else if (propVal is double[])
                    {
                        var values = propVal as double[];
                        var arrValue = new ArrayValue();
                        foreach (var val in values)
                        {
                            arrValue.Values.Add(new Value()
                            {
                                DoubleValue = val
                            });
                        }

                        entity[property.Name] = arrValue;
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }

                // If the property implements IDatastoreEntity then the built in 
                // functionality to populate the entity
                else if (propertyInterfaces.Contains(typeof(IDatastoreEntityAndJsonBinding)))
                {
                    var propVal = type.GetProperty(propertyType.Name).GetValue(this) as IDatastoreEntityAndJsonBinding;
                    entity[property.Name] = propVal.ToEntity();
                }
            }

            return entity;
        }

        public void Save(DatastoreDb db)
        {
            if (Id == 0)
                throw new Exception("The Id has to be set before saving to the datastore.");

            db.Upsert(ToEntity());
        }

        public static TieFighterDatastoreContext DatastoreDbReference { get; set; }
    }
}
