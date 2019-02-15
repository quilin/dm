using DM.Services.Authentication.Dto;

namespace DM.Services.Authentication.Implementation
{
    public interface IIdentitySetter
    {
        IIdentity Current { set; }
    }
}