using System;
using AutoMapper;
using DM.Services.DataAccess;
using DM.Services.DataAccess.MongoIntegration;
using Microsoft.EntityFrameworkCore;

namespace DM.Tests.Core;

public abstract class DbTestBase : UnitTestBase
{
    protected static IMapper GetMapper() => new Mapper(new MapperConfiguration(c => { }));

    protected static DmDbContext GetRdb(string name) => new(
        new DbContextOptionsBuilder<DmDbContext>().UseInMemoryDatabase(name).Options);

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