using System;
using AutoMapper;
using DM.Services.Core.Configuration;
using DM.Services.DataAccess;
using DM.Services.DataAccess.MongoIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DM.Tests.Core;

public abstract class DbTestBase : UnitTestBase
{
    protected static IMapper GetMapper() => new Mapper(new MapperConfiguration(c => { }));

    protected static DmDbContext GetRdb(string name) => new(
        new DbContextOptionsBuilder<DmDbContext>().UseInMemoryDatabase(name).Options);

    protected MongoDbWrapper GetMongoClient(string name)
    {
        var connectionStrings = Mock<IOptions<ConnectionStrings>>();
        connectionStrings
            .Setup(s => s.Value)
            .Returns(new ConnectionStrings
            {
                Mongo = $"mongodb://localhost:27017/integration_tests_{name}"
            });
        return new MongoDbWrapper(name, new DmMongoClient(connectionStrings.Object));
    }

    public class MongoDbWrapper : IDisposable
    {
        private readonly string name;
        public readonly DmMongoClient Client;

        public MongoDbWrapper(string name, DmMongoClient client)
        {
            this.name = name;
            Client = client;
        }
            
        public void Dispose()
        {
            Client?.DropDatabase($"integration_tests_{name}");
        }
    }
}