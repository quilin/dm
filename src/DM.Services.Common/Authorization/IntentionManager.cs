using System;
using System.Collections.Generic;
using System.Linq;
using DM.Services.Authentication.Implementation.UserIdentity;
using Microsoft.Extensions.Logging;

namespace DM.Services.Common.Authorization;

/// <inheritdoc />
internal class IntentionManager : IIntentionManager
{
    private readonly IIdentityProvider identityProvider;
    private readonly IEnumerable<IIntentionResolver> resolvers;
    private readonly ILogger<IntentionManager> logger;

    /// <inheritdoc />
    public IntentionManager(
        IIdentityProvider identityProvider,
        IEnumerable<IIntentionResolver> resolvers,
        ILogger<IntentionManager> logger)
    {
        this.identityProvider = identityProvider;
        this.resolvers = resolvers;
        this.logger = logger;
    }

    /// <inheritdoc />
    public bool IsAllowed<TIntention>(TIntention intention) where TIntention : struct
    {
        var matchingResolver = resolvers
            .OfType<IIntentionResolver<TIntention>>()
            .FirstOrDefault();
        if (matchingResolver != null)
        {
            return matchingResolver.IsAllowed(identityProvider.Current.User, intention);
        }

        logger.LogError("No matching resolver found for intention type {intentionType}", typeof(TIntention));
        return false;
    }

    /// <inheritdoc />
    public bool IsAllowed<TIntention, TTarget>(TIntention intention, TTarget target)
        where TIntention : struct
    {
        var matchingResolver = resolvers
            .OfType<IIntentionResolver<TIntention, TTarget>>()
            .FirstOrDefault();
        if (matchingResolver != null)
        {
            return matchingResolver.IsAllowed(identityProvider.Current.User, intention, target);
        }

        logger.LogError(
            "No matching resolver found for intention type {intentionType} and target type {targetType}",
            typeof(TIntention), typeof(TTarget));
        return false;
    }

    /// <inheritdoc />
    public void ThrowIfForbidden<TIntention>(TIntention intention) where TIntention : struct
    {
        if (!IsAllowed(intention))
        {
            throw new IntentionManagerException(identityProvider.Current.User, GetIntentionEnum(intention));
        }
    }

    /// <inheritdoc />
    public void ThrowIfForbidden<TIntention, TTarget>(TIntention intention, TTarget target)
        where TIntention : struct
    {
        if (!IsAllowed(intention, target))
        {
            throw new IntentionManagerException(identityProvider.Current.User, GetIntentionEnum(intention), target);
        }
    }

    private static Enum GetIntentionEnum<TIntention>(TIntention intention)
    {
        return (Enum) Enum.Parse(typeof(TIntention), intention.ToString());
    }
}