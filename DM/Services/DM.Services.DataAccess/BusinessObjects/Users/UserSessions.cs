using System;
using System.Collections.Generic;
using DM.Services.DataAccess.MongoIntegration;

namespace DM.Services.DataAccess.BusinessObjects.Users
{
    [MongoCollectionName("UserSessions")]
    public class UserSessions
    {
        public Guid Id { get; set; }
        public List<Session> Sessions { get; set; }
    }

    public class Session
    {
        public Guid Id { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsPersistent { get; set; }
    }
}