using System;

namespace DM.Services.DataAccess.MongoIntegration
{
    /// <summary>
    /// Attribute for Mongo collection name mapping
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class MongoCollectionNameAttribute : Attribute
    {
        /// <summary>
        /// Desired collection name
        /// </summary>
        public string CollectionName { get; }

        public MongoCollectionNameAttribute(string collectionName)
        {
            CollectionName = collectionName;
        }
    }
}