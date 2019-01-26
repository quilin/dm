using System;
using System.Reflection;
using MongoDB.Driver;

namespace DM.Services.DataAccess.MongoIntegration
{
    public class DmMongoClient : MongoClient
    {
        private readonly string databaseName;

        private IMongoDatabase Database => GetDatabase(databaseName);

        public DmMongoClient(string connectionString) : base(connectionString)
        {
            var uri = new Uri(connectionString);
            databaseName = uri.AbsolutePath.Trim('/');
        }

        public IMongoCollection<T> GetCollection<T>() =>
            Database.GetCollection<T>(typeof(T).GetCustomAttribute<MongoCollectionNameAttribute>().CollectionName);
    }
}