using System;

namespace DM.Services.DataAccess.MongoIntegration;

/// <summary>
/// Attribute for Mongo collection name mapping
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
internal class MongoCollectionNameAttribute : Attribute
{
    /// <summary>
    /// Desired collection name
    /// </summary>
    public string CollectionName { get; }

    /// <inheritdoc />
    public MongoCollectionNameAttribute(string collectionName)
    {
        CollectionName = collectionName;
    }
}