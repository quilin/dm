using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.Authentication.Factories
{
    public interface ISessionFactory
    {
        Session Create(bool persistent);
    }
}