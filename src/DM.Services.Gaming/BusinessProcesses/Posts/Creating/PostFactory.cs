using System;
using DM.Services.Core.Implementation;
using DM.Services.DataAccess.BusinessObjects.Games.Posts;
using DM.Services.Gaming.Dto.Input;

namespace DM.Services.Gaming.BusinessProcesses.Posts.Creating;

/// <inheritdoc />
internal class PostFactory : IPostFactory
{
    private readonly IGuidFactory guidFactory;
    private readonly IDateTimeProvider dateTimeProvider;

    /// <inheritdoc />
    public PostFactory(
        IGuidFactory guidFactory,
        IDateTimeProvider dateTimeProvider)
    {
        this.guidFactory = guidFactory;
        this.dateTimeProvider = dateTimeProvider;
    }

    /// <inheritdoc />
    public Post Create(CreatePost createPost, Guid userId)
    {
        return new Post
        {
            PostId = guidFactory.Create(),
            CreateDate = dateTimeProvider.Now,
            UserId = userId,
            RoomId = createPost.RoomId,
            CharacterId = createPost.CharacterId,
            Text = createPost.Text,
            Commentary = createPost.Commentary,
            MasterMessage = createPost.MasterMessage,
            IsRemoved = false,
            LastUpdateDate = null
        };
    }
}