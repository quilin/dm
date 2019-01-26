using System;

namespace DM.Services.DataAccess.BusinessObjects.Administration
{
    public interface IAdministrated
    {
        Guid UserId { get; set; }
        Guid ModeratorId { get; set; }
    }
}