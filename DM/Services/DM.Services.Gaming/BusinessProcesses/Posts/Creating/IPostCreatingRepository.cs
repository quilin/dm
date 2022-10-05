using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Gaming.Dto.Output;
using DbPost = DM.Services.DataAccess.BusinessObjects.Games.Posts.Post;
using PendingPost = DM.Services.DataAccess.BusinessObjects.Games.Links.PendingPost;

namespace DM.Services.Gaming.BusinessProcesses.Posts.Creating;

/// <summary>
/// Storage for post creating
/// </summary>
internal interface IPostCreatingRepository
{
    /// <summary>
    /// Create new game post
    /// </summary>
    /// <param name="post">Post DAL model</param>
    /// <param name="pendingPostUpdates">Pending post changes</param>
    /// <returns></returns>
    Task<Post> Create(DbPost post, IEnumerable<IUpdateBuilder<PendingPost>> pendingPostUpdates);
}