using System;
using System.Threading.Tasks;
using DM.Services.DataAccess;
using DM.Services.DataAccess.MongoIntegration;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Testcontainers.MongoDb;
using Testcontainers.PostgreSql;
using Xunit;

namespace DM.Services.Forum.Tests.BusinessProcesses.Fora;

public class DbFixture : IAsyncLifetime
{
    private const string DatabaseNamePlaceholder = nameof(DatabaseNamePlaceholder);
    private readonly PostgreSqlContainer pgContainer = new PostgreSqlBuilder()
        .WithDatabase(DatabaseNamePlaceholder)
        .Build();

    private readonly MongoDbContainer mongoContainer = new MongoDbBuilder().Build();

    public Task InitializeAsync() => Task.WhenAll(
        pgContainer.StartAsync(),
        mongoContainer.StartAsync());

    public async Task<(DmDbContext Rdb, DmMongoClient mongo)> CreateDbs(string dbName = null)
    {
        dbName ??= Guid.NewGuid().ToString();

        var pgConnectionString = pgContainer.GetConnectionString()
            .Replace(DatabaseNamePlaceholder, $"postgres-{dbName}");
        var dbContextOptions = new DbContextOptionsBuilder<DmDbContext>().UseNpgsql(pgConnectionString).Options;
        var dbContext = new DmDbContext(dbContextOptions);
        await dbContext.Database.MigrateAsync();

        var mongoConnectionString = $"{mongoContainer.GetConnectionString()}/{dbName}";
        var mongoClient = new DmMongoClient(
            MongoClientSettings.FromConnectionString(mongoConnectionString), mongoConnectionString);

        return (dbContext, mongoClient);
    }

    public Task DisposeAsync() => Task.WhenAll(
        pgContainer.DisposeAsync().AsTask(),
        mongoContainer.DisposeAsync().AsTask());
}

[CollectionDefinition(nameof(DbCollection))]
public class DbCollection : ICollectionFixture<DbFixture>
{
}