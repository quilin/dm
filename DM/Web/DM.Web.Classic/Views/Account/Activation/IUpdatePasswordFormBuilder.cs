using System;
using System.Threading.Tasks;

namespace DM.Web.Classic.Views.Account.Activation
{
    public interface IUpdatePasswordFormBuilder
    {
        Task<UpdatePasswordForm> Build(Guid token);
    }
}