using System.Threading.Tasks;
using DM.Services.DataAccess;
using DM.Services.DataAccess.BusinessObjects.Users;
using DM.Services.DataAccess.BusinessObjects.Users.Settings;
using DM.Services.DataAccess.MongoIntegration;
using DM.Services.DataAccess.RelationalStorage;
using MongoDB.Driver;

namespace DM.Services.Community.BusinessProcesses.Users.Updating
{
    /// <inheritdoc />
    public class UserUpdatingRepository : MongoCollectionRepository<UserSettings>, IUserUpdatingRepository
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

            var (id, updateSettings) = settingsUpdate.DefineUpdateTo(mongoClient);
            await mongoClient.GetCollection<UserSettings>()
                .UpdateOneAsync(Filter.Eq(u => u.Id, id), updateSettings,
                    new UpdateOptions {IsUpsert = true});
        }
    }
}