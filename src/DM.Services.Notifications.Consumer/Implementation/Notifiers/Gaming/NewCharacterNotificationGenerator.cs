using System;
using System.Collections.Generic;
using System.Linq;
using DM.Services.Core.Dto.Enums;
using DM.Services.Core.Extensions;
using DM.Services.DataAccess;
using DM.Services.Notifications.Dto;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.Notifications.Consumer.Implementation.Notifiers.Gaming;

/// <inheritdoc />
internal class NewCharacterNotificationGenerator : BaseNotificationGenerator
{
    private readonly DmDbContext dbContext;

    /// <inheritdoc />
    public NewCharacterNotificationGenerator(
        DmDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    /// <inheritdoc />
    protected override EventType EventType => EventType.NewCharacter;

    /// <inheritdoc />
    public override async IAsyncEnumerable<CreateNotification> Generate(Guid entityId)
    {
        var data = await dbContext.Characters
            .Where(c => c.CharacterId == entityId)
            .Select(c => new
            {
                c.GameId,
                c.Game.Title,
                c.Author.Login,
                c.Author.UserId,
                c.Game.MasterId,
                c.Game.AssistantId
            })
            .FirstAsync();

        var usersInterested = new List<Guid> {data.MasterId};
        if (data.AssistantId.HasValue)
        {
            usersInterested.Add(data.AssistantId.Value);
        }

        usersInterested.Remove(data.UserId);
        if (!usersInterested.Any())
        {
            yield break;
        }

        yield return new CreateNotification
        {
            UsersInterested = usersInterested,
            Metadata = new
            {
                AuthorLogin = data.Login,
                GameTitle = data.Title,
                GameId = data.GameId.EncodeToReadable()
            }
        };
    }
}