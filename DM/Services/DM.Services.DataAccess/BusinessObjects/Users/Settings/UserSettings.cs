using System;
using DM.Services.DataAccess.MongoIntegration;

namespace DM.Services.DataAccess.BusinessObjects.Users.Settings
{
    [MongoCollectionName("UserSettings")]
    public class UserSettings
    {
        public Guid Id { get; set; }
        public PagingSettings Paging { get; set; }
        public string NannyGreetingsMessage { get; set; }
        public ColorScheme ColorScheme { get; set; }
    }
}