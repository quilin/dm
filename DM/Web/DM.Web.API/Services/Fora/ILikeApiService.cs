using System;
using System.Threading.Tasks;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Users;

namespace DM.Web.API.Services.Fora;

/// <summary>
/// API service for forum likes
/// </summary>
public interface ILikeApiService
{
    /// <summary>
    /// Like the topic
    /// </summary>
    /// <param name="topicId">Topic identifier</param>
    /// <returns>Envelope for user who just liked the topic</returns>
    Task<Envelope<User>> LikeTopic(Guid topicId);

    /// <summary>
    /// Remove user's like from topic
    /// </summary>
    /// <param name="topicId">Topic identifier</param>
    /// <returns></returns>
    Task DislikeTopic(Guid topicId);

    /// <summary>
    /// Like the commentary
    /// </summary>
    /// <param name="commentId">Commentary identifier</param>
    /// <returns>Envelope for user who just liked the commentary</returns>
    Task<Envelope<User>> LikeComment(Guid commentId);

    /// <summary>
    /// Remove user's like from commentary
    /// </summary>
    /// <param name="commentId">Commentary identifier</param>
    /// <returns></returns>
    Task DislikeComment(Guid commentId);
}