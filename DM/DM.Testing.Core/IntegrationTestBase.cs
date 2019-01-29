using System;
using DM.Services.DataAccess;
using DM.Services.DataAccess.MongoIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace DM.Testing.Core
{
    public abstract class IntegrationTestBase : UnitTestBase
    {
        protected readonly DmMongoClient DmMongoClient;
        protected readonly DmDbContext DmDbContext;

        protected IntegrationTestBase(ITestOutputHelper testOutputHelper) : base()
        {
            DmMongoClient = new DmMongoClient("mongodb://localhost:27017/dm3release?maxPoolSize=1000");
            DmDbContext = new DmDbContext(new DbContextOptionsBuilder<DmDbContext>()
                .UseNpgsql(
                    "User ID=postgres;Password=admin;Host=localhost;Port=5432;Database=dm3;Pooling=true;MinPoolSize=0;MaxPoolSize=100;Connection Idle Lifetime=60;")
                .UseLoggerFactory(new LoggerFactory(new[] {new XUnitLoggerProvider(testOutputHelper)}))
                .Options);
        }

        private class XUnitLoggerProvider : ILoggerProvider
        {
            private readonly ITestOutputHelper testOutputHelper;

            public XUnitLoggerProvider(ITestOutputHelper testOutputHelper)
            {
                this.testOutputHelper = testOutputHelper;
            }

            public ILogger CreateLogger(string categoryName)
                => new XUnitLogger(testOutputHelper, categoryName);

            public void Dispose()
            {
            }
        }

        private class XUnitLogger : ILogger
        {
            private readonly ITestOutputHelper testOutputHelper;
            private readonly string categoryName;

            public XUnitLogger(ITestOutputHelper testOutputHelper, string categoryName)
            {
                this.testOutputHelper = testOutputHelper;
                this.categoryName = categoryName;
            }

            public IDisposable BeginScope<TState>(TState state)
                => NoopDisposable.Instance;

            public bool IsEnabled(LogLevel logLevel)
                => true;

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
                Func<TState, Exception, string> formatter)
            {
                testOutputHelper.WriteLine($"{categoryName} [{eventId}] {formatter(state, exception)}");
                if (exception != null)
                    testOutputHelper.WriteLine(exception.ToString());
            }

            private class NoopDisposable : IDisposable
            {
                public static readonly NoopDisposable Instance = new NoopDisposable();

                public void Dispose()
                {
                }
            }
        }
    }
}