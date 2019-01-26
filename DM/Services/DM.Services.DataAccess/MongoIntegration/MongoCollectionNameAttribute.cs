using System;

namespace DM.Services.DataAccess.MongoIntegration
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MongoCollectionNameAttribute : Attribute
    {
        public string CollectionName { get; }

        public MongoCollectionNameAttribute(string collectionName)
        {
            CollectionName = collectionName;
        }
    }
}