using System;
using System.Reflection;
using DM.Services.Core.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DM.Services.DataAccess.MongoIntegration
{
    public class DmMongoClient : MongoClient
    {
        private readonly string databaseName;

        private IMongoDatabase Database => GetDatabase(databaseName);

        public DmMongoClient(IOptions<ConnectionStrings> options) : base(options.Value.DmMongoClient)
        {
            var uri = new Uri(options.Value.DmMongoClient);
            databaseName = uri.AbsolutePath.Trim('/');
        }

        public IMongoCollection<T> GetCollection<T>() =>
            Database.GetCollection<T>(typeof(T).GetCustomAttribute<MongoCollectionNameAttribute>().CollectionName);
    }
}