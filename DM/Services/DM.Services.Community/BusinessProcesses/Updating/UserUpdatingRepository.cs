using System.Threading.Tasks;
using DM.Services.DataAccess;
using DM.Services.DataAccess.BusinessObjects.Users;
using DM.Services.DataAccess.BusinessObjects.Users.Settings;
using DM.Services.DataAccess.MongoIntegration;
using DM.Services.DataAccess.RelationalStorage;

namespace DM.Services.Community.BusinessProcesses.Updating
{
    /// <inheritdoc />
    public class UserUpdatingRepository : IUserUpdatingRepository
    {
        private readonly DmDbContext dbContext;
        private readonly DmMongoClient mongoClient;

        /// <inheritdoc />
        public UserUpdatingRepository(
            DmDbContext dbContext,
            DmMongoClient mongoClient)
        {
            this.dbContext = dbContext;
            this.mongoClient = mongoClient;
        }
        
        /// <inheritdoc />
        public async Task Update(IUpdateBuilder<User> updateUser, IUpdateBuilder<UserSettings> settingsUpdate)
        {
            updateUser.AttachTo(dbContext);
            await updateUser.SaveTo(mongoClient);
            await dbContext.SaveChangesAsync();
        }
    }
}