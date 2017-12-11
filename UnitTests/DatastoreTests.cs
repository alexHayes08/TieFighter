using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TieFighter.Models;
using static TieFighter.Startup;

namespace UnitTests
{
    [TestClass]
    public class DatastoreTests
    {
        [TestMethod]
        public void TestGetEntityKey()
        {
            const string keyLiteral = "552cfc72-d51d-4ab1-94cb-16147c689258";
            var key = DatastoreDb.UsersKeyFactory.CreateKey(keyLiteral);
            var entity = DatastoreDb.Db.Lookup(key);
            var stringKey = entity.GetEntityKey();
            if (stringKey != keyLiteral)
                throw new Exception();
        }

        [TestMethod]
        public void TestEntityToObject()
        {
            const string keyLiteral = "552cfc72-d51d-4ab1-94cb-16147c689258";
            var key = DatastoreDb.UsersKeyFactory.CreateKey(keyLiteral);
            var entity = DatastoreDb.Db.Lookup(key);
            var user = DatastoreHelpers.ParseEntityToObject<User>(entity);

            if (user.Id != keyLiteral)
                throw new Exception();
        }
    }
}
