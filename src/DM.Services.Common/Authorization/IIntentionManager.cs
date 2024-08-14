namespace DM.Services.Common.Authorization;

/// <summary>
/// Central authorization endpoint
/// </summary>
public interface IIntentionManager
{
    /// <summary>
    /// Tells if current user is allowed to perform certain action
    /// </summary>
    /// <param name="intention">Intended action</param>
    /// <typeparam name="TIntention">Type of intention</typeparam>
    /// <returns>Whether user is allowed to perform the action</returns>
    bool IsAllowed<TIntention>(TIntention intention) where TIntention : struct;

    /// <summary>
    /// Tells if current user is allowed to perform certain action
    /// </summary>
    /// <param name="intention">Intended action</param>
    /// <param name="target">Object of the action</param>
    /// <typeparam name="TIntention">Type of intention</typeparam>
    /// <typeparam name="TTarget">Type of action object</typeparam>
    /// <returns>Whether user is allowed to perform the action</returns>
    bool IsAllowed<TIntention, TTarget>(TIntention intention, TTarget target) where TIntention : struct;

    /// <summary>
    /// Throws the specific exception <see cref="IntentionManagerException"/> if user is not allowed to perform certain action
    /// </summary>
    /// <param name="intention">Intended action</param>
    /// <typeparam name="TIntention">Type of intention</typeparam>
    /// <returns></returns>
    void ThrowIfForbidden<TIntention>(TIntention intention) where TIntention : struct;

    /// <summary>
    /// Throws the specific exception <see cref="IntentionManagerException"/> if user is not allowed to perform certain action
    /// </summary>
    /// <param name="intention">Intended action</param>
    /// <param name="target">Object of the action</param>
    /// <typeparam name="TIntention">Type of intention</typeparam>
    /// <typeparam name="TTarget">Type of action object</typeparam>
    /// <returns></returns>
    void ThrowIfForbidden<TIntention, TTarget>(TIntention intention, TTarget target) where TIntention : struct;
}