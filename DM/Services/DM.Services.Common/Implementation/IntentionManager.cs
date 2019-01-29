using System;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.Authentication.Implementation;

namespace DM.Services.Common.Implementation
{
    public class IntentionManager : IIntentionManager
    {
        private readonly IUserProvider userProvider;
        private readonly IIntentionResolver[] resolvers;

        public IntentionManager(
            IUserProvider userProvider,
            IIntentionResolver[] resolvers)
        {
            this.userProvider = userProvider;
            this.resolvers = resolvers;
        }

        public async Task<bool> IsAllowed<TIntention, TTarget>(TIntention intention, TTarget target)
            where TIntention : struct
            where TTarget : class
        {
            var matchingResolver = resolvers
                .OfType<IIntentionResolver<TIntention, TTarget>>()
                .FirstOrDefault();
            return matchingResolver != null &&
                   await matchingResolver.IsAllowed(userProvider.Current, intention, target);
        }

        public async Task ThrowIfForbidden<TIntention, TTarget>(TIntention intention, TTarget target)
            where TIntention : struct
            where TTarget : class
        {
            if (!await IsAllowed(intention, target))
            {
                throw new IntentionManagerException(userProvider.Current, GetIntentionEnum(intention), target);
            }
        }

        private static Enum GetIntentionEnum<TIntention>(TIntention intention)
        {
            return (Enum) Enum.Parse(typeof(TIntention), intention.ToString());
        }
    }
}