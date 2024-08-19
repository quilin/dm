using DM.Services.Community.BusinessProcesses.Users.Updating;
using DM.Services.DataAccess;
using DM.Services.DataAccess.BusinessObjects.Users;
using DM.Services.DataAccess.BusinessObjects.Users.Settings;
using DM.Services.DataAccess.MongoIntegration;
using DM.Services.DataAccess.RelationalStorage;

namespace DM.Services.Community.Storage.Storages.Users;

/// <inheritdoc />
internal class UserUpdatingRepository(
    DmDbContext dbContext,
    DmMongoClient client) : MongoCollectionRepository<UserSettings>(client), IUserUpdatingRepository
{
    private readonly DmMongoClient mongoClient = client;

    /// <inheritdoc />
    public async Task UpdateUser(IUpdateBuilder<User> updateUser, IUpdateBuilder<UserSettings> settingsUpdate,
        CancellationToken cancellationToken)
    {
        updateUser.AttachTo(dbContext);
        await dbContext.SaveChangesAsync(cancellationToken);
        await settingsUpdate.UpdateFor(mongoClient, true, cancellationToken);
    }
}