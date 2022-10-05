using DM.Services.Authentication.Dto;

namespace DM.Services.Common.Authorization;

/// <summary>
/// Resolves if user is allowed to perform action of specific type upon object of specific type
/// </summary>
public interface IIntentionResolver
{
}

/// <summary>
/// Resolves if user is allowed to perform action of specific type upon object of specific type
/// </summary>
/// <typeparam name="TIntention">Action type</typeparam>
/// <typeparam name="TTarget">Object type</typeparam>
public interface IIntentionResolver<in TIntention, in TTarget> : IIntentionResolver
    where TIntention : struct
{
    /// <summary>
    /// Tells if the user is allowed to perform certain action upon certain object
    /// </summary>
    /// <param name="user">Action subject</param>
    /// <param name="intention">Action</param>
    /// <param name="target">Action object</param>
    /// <returns>Whether subject is allowed to perform action upon object</returns>
    bool IsAllowed(AuthenticatedUser user, TIntention intention, TTarget target);
}

/// <summary>
/// Resolves if user is allowed to perform action of specific type
/// </summary>
/// <typeparam name="TIntention">Action type</typeparam>
public interface IIntentionResolver<in TIntention> : IIntentionResolver
    where TIntention : struct
{
    /// <summary>
    /// Tells if the user is allowed to perform certain action
    /// </summary>
    /// <param name="user">Action subject</param>
    /// <param name="intention">Action</param>
    /// <returns>Whether subject is allowed to perform action upon object</returns>
    bool IsAllowed(AuthenticatedUser user, TIntention intention);
}