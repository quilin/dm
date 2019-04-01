using System;
using System.Reflection;
using DM.Services.Core.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DM.Services.DataAccess.MongoIntegration
{
    /// <summary>
    /// Mongo DB client wrapper
    /// </summary>
    public class DmMongoClient : MongoClient
    {
        private readonly string databaseName;

        private IMongoDatabase Database => GetDatabase(databaseName);

        /// <inheritdoc />
        public DmMongoClient(IOptions<ConnectionStrings> options) : base(options.Value.DmMongoClient)
        {
            var uri = new Uri(options.Value.DmMongoClient);
            databaseName = uri.AbsolutePath.Trim('/');
        }

        /// <summary>
        /// Get collection for entity type
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <returns>Mongo collection</returns>
        public IMongoCollection<T> GetCollection<T>() =>
            Database.GetCollection<T>(typeof(T).GetCustomAttribute<MongoCollectionNameAttribute>().CollectionName);
    }
}