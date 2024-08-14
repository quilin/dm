using System;
using System.Threading.Tasks;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Users;

namespace DM.Web.API.Services.Gaming;

/// <summary>
/// API service for game likes
/// </summary>
public interface ILikeApiService
{
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