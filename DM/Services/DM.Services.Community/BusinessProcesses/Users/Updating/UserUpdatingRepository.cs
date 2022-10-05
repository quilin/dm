using System.Threading.Tasks;
using DM.Services.DataAccess;
using DM.Services.DataAccess.BusinessObjects.Users;
using DM.Services.DataAccess.BusinessObjects.Users.Settings;
using DM.Services.DataAccess.MongoIntegration;
using DM.Services.DataAccess.RelationalStorage;

namespace DM.Services.Community.BusinessProcesses.Users.Updating;

/// <inheritdoc />
internal class UserUpdatingRepository : MongoCollectionRepository<UserSettings>, IUserUpdatingRepository
{
    private readonly DmDbContext dbContext;
    private readonly DmMongoClient mongoClient;

    /// <inheritdoc />
    public UserUpdatingRepository(
        DmDbContext dbContext,
        DmMongoClient mongoClient) : base(mongoClient)
    {
        this.dbContext = dbContext;
        this.mongoClient = mongoClient;
    }

    /// <inheritdoc />
    public async Task UpdateUser(IUpdateBuilder<User> updateUser, IUpdateBuilder<UserSettings> settingsUpdate)
    {
        updateUser.AttachTo(dbContext);
        await dbContext.SaveChangesAsync();
        await settingsUpdate.UpdateFor(mongoClient, true);
    }
}