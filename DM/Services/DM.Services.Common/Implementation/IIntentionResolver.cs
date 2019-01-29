using System.Threading.Tasks;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.Common.Implementation
{
    public interface IIntentionResolver
    {
    }

    public interface IIntentionResolver<in TIntention, in TTarget> : IIntentionResolver
        where TIntention : struct
        where TTarget : class
    {
        Task<bool> IsAllowed(IUser user, TIntention intention, TTarget target);
    }
}