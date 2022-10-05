using System;
using DM.Services.Core.Dto;

namespace DM.Services.Gaming.Dto.Output;

/// <inheritdoc />
public class Reader : GeneralUser
{
    /// <summary>
    /// Reader identifier
    /// </summary>
    public Guid ReaderId { get; set; }
}