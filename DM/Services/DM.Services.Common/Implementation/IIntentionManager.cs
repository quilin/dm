using System.Threading.Tasks;

namespace DM.Services.Common.Implementation
{
    public interface IIntentionManager
    {
        Task<bool> IsAllowed<TIntention, TTarget>(TIntention intention, TTarget target)
            where TIntention : struct
            where TTarget : class;

        Task ThrowIfForbidden<TIntention, TTarget>(TIntention intention, TTarget target)
            where TIntention : struct
            where TTarget : class;
    }
}