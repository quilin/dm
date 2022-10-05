using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Dto;
using DM.Services.Core.Dto;
using DM.Services.Core.Dto.Enums;
using DM.Services.Core.Exceptions;
using DM.Services.MessageQueuing.GeneralBus;

namespace DM.Services.Common.BusinessProcesses.Likes;

/// <summary>
/// Common part of like service
/// </summary>
public abstract class LikeServiceBase
{
    private readonly IIdentityProvider identityProvider;
    private readonly ILikeFactory likeFactory;
    private readonly ILikeRepository likeRepository;
    private readonly IInvokedEventProducer producer;

    /// <inheritdoc />
    protected LikeServiceBase(
        IIdentityProvider identityProvider,
        ILikeFactory likeFactory,
        ILikeRepository likeRepository,
        IInvokedEventProducer producer)
    {
        this.identityProvider = identityProvider;
        this.likeFactory = likeFactory;
        this.likeRepository = likeRepository;
        this.producer = producer;
    }
        
    /// <summary>
    /// Like certain entity
    /// </summary>
    /// <param name="entity">Likable entity</param>
    /// <param name="eventType">Event type to generate after like</param>
    /// <returns>Person who liked</returns>
    protected async Task<GeneralUser> Like(ILikable entity, EventType eventType)
    {
        var currentUser = identityProvider.Current.User;
        if (entity.Likes.Any(l => l.UserId == currentUser.UserId))
        {
            throw new HttpException(HttpStatusCode.Conflict,
                $"User already liked this {entity.GetType().Name.ToLower()}");
        }

        var like = likeFactory.Create(entity.Id, currentUser.UserId);
        await likeRepository.Add(like);
        await producer.Send(eventType, like.LikeId);
        return currentUser;
    }

    /// <summary>
    /// Dislike certain entity
    /// </summary>
    /// <param name="entity">Likable entity</param>
    /// <returns></returns>
    protected async Task Dislike(ILikable entity)
    {
        var currentUser = identityProvider.Current.User;
        if (entity.Likes.All(l => l.UserId != currentUser.UserId))
        {
            throw new HttpException(HttpStatusCode.Conflict,
                $"User never liked this {entity.GetType().Name.ToLower()} in the first place");
        }

        await likeRepository.Delete(entity.Id, currentUser.UserId);
    }
}