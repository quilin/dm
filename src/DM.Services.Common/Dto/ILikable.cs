using System;
using System.Collections.Generic;
using DM.Services.Core.Dto;

namespace DM.Services.Common.Dto;

/// <summary>
/// Entity that carries user likes
/// </summary>
public interface ILikable
{
    /// <summary>
    /// Entity identifier
    /// </summary>
    Guid Id { get; }

    /// <summary>
    /// List of users who liked the entity
    /// </summary>
    IEnumerable<GeneralUser> Likes { get; }
}