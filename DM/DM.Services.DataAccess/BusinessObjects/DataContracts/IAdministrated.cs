using System;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.DataAccess.BusinessObjects.DataContracts
{
    public interface IAdministrated : IRemovable
    {
        Guid UserId { get; set; }
        Guid ModeratorId { get; set; }

        User User { get; set; }
        User Moderator { get; set; }
    }
}