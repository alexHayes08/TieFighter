using Google.Cloud.Datastore.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
namespace TieFighter.Models
{
    public interface IDatastoreContext
    {
        KeyFactory GetKeyFactoryFor(string kind);
        //KeyFactory GetKeyFactoryFor(Type kind, params object[] ancestors);
        //KeyFactory GetKeyFactoryFor<T>(T kind, params object[] ancestors) where T : class;
        DatastoreDb Db { get; }
    }

    public class ObjectAndAssemblyQualifiedName
    {
        public ObjectAndAssemblyQualifiedName(Object obj)
        {
            AssemblyQualifiedName = obj.GetType().AssemblyQualifiedName;
            Object = obj;
        }

        public string AssemblyQualifiedName { get; private set; }
        public object Object { get; private set; }
    }

    public class DemoDatastoreContext : IDatastoreContext
    {
        #region Constructors

        public DemoDatastoreContext(string projectId)
        {
            Db = DatastoreDb.Create(projectId);
            registeredKeyFactories = new Dictionary<string, KeyFactory>();
        }

        #endregion

        #region Fields

        private Dictionary<string, KeyFactory> registeredKeyFactories;

        #endregion

        #region Properties

        public DatastoreDb Db { get; private set; }

        #endregion

        #region Functions

        public KeyFactory GetKeyFactoryFor(string kind)
        {
            throw new NotImplementedException();
        }

        //public KeyFactory GetKeyFactoryFor(Type kind, params object[] ancestors)
        //{
        //    string kindName = Path.Combine(kind.AssemblyQualifiedName);

        //    if (registeredKeyFactories.Keys.Contains(kindName))
        //    {
        //        return registeredKeyFactories[kindName];
        //    }
        //    else
        //    {
        //        KeyFactory currentKeyFactory = null;
        //        if (ancestors.Length > 0)
        //        {
        //            currentKeyFactory = Db.CreateKeyFactory(ancestors[0]);
        //            for (var i = 1; i < ancestors.Length; i++)
        //            {

        //            }
        //        }
        //        else
        //        {
        //            var keyFactory = Db.CreateKeyFactory(kind.AssemblyQualifiedName);
        //            registeredKeyFactories[kind.AssemblyQualifiedName] = keyFactory;
        //            return keyFactory;
        //        }
        //    }
        //}

        #endregion
    }
}
