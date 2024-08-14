using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DM.Services.DataAccess.Design;

/// <inheritdoc />
internal class DmDbContextFactory : IDesignTimeDbContextFactory<DmDbContext>
{
    /// <inheritdoc />
    public DmDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("DM_ConnectionStrings__Rdb") ??
                               throw new ArgumentNullException();
        return new DmDbContext(new DbContextOptionsBuilder<DmDbContext>()
            .UseNpgsql(connectionString).Options);
    }
}