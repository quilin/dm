using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.UserIdentity;

namespace DM.Services.Common.Authorization
{
    /// <inheritdoc />
    public class IntentionManager : IIntentionManager
    {
        private readonly IIdentityProvider identityProvider;
        private readonly IEnumerable<IIntentionResolver> resolvers;

        /// <inheritdoc />
        public IntentionManager(
            IIdentityProvider identityProvider,
            IEnumerable<IIntentionResolver> resolvers)
        {
            this.identityProvider = identityProvider;
            this.resolvers = resolvers;
        }

        /// <inheritdoc />
        public async Task<bool> IsAllowed<TIntention>(TIntention intention) where TIntention : struct
        {
            var matchingResolver = resolvers
                .OfType<IIntentionResolver<TIntention>>()
                .FirstOrDefault();
            return matchingResolver != null &&
                await matchingResolver.IsAllowed(identityProvider.Current.User, intention);
        }

        /// <inheritdoc />
        public async Task<bool> IsAllowed<TIntention, TTarget>(TIntention intention, TTarget target)
            where TIntention : struct
            where TTarget : class
        {
            var matchingResolver = resolvers
                .OfType<IIntentionResolver<TIntention, TTarget>>()
                .FirstOrDefault();
            return matchingResolver != null &&
                   await matchingResolver.IsAllowed(identityProvider.Current.User, intention, target);
        }

        /// <inheritdoc />
        public async Task<bool> IsAllowed<TIntention, TTarget, TSubTarget>(
            TIntention intention, TTarget target, TSubTarget subTarget)
            where TIntention : struct
            where TTarget : class
            where TSubTarget : class
        {
            var matchingResolver = resolvers
                .OfType<IIntentionResolver<TIntention, TTarget, TSubTarget>>()
                .FirstOrDefault();
            return matchingResolver != null &&
                await matchingResolver.IsAllowed(identityProvider.Current.User, intention, target, subTarget);
        }

        /// <inheritdoc />
        public async Task ThrowIfForbidden<TIntention>(TIntention intention) where TIntention : struct
        {
            if (!await IsAllowed(intention))
            {
                throw new IntentionManagerException(identityProvider.Current.User, GetIntentionEnum(intention));
            }
        }

        /// <inheritdoc />
        public async Task ThrowIfForbidden<TIntention, TTarget>(TIntention intention, TTarget target)
            where TIntention : struct
            where TTarget : class
        {
            if (!await IsAllowed(intention, target))
            {
                throw new IntentionManagerException(identityProvider.Current.User, GetIntentionEnum(intention), target);
            }
        }

        /// <inheritdoc />
        public async Task ThrowIfForbidden<TIntention, TTarget, TSubTarget>(
            TIntention intention, TTarget target, TSubTarget subTarget)
            where TIntention : struct
            where TTarget : class
            where TSubTarget : class
        {
            if (!await IsAllowed(intention, target, subTarget))
            {
                throw new IntentionManagerException(identityProvider.Current.User, GetIntentionEnum(intention), target);
            }
        }

        private static Enum GetIntentionEnum<TIntention>(TIntention intention)
        {
            return (Enum) Enum.Parse(typeof(TIntention), intention.ToString());
        }
    }
}