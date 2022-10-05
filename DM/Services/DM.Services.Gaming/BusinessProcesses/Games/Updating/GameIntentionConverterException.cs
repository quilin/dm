using System;
using DM.Services.Core.Dto.Enums;

namespace DM.Services.Gaming.BusinessProcesses.Games.Updating;

/// <summary>
/// Argument out of range exception for game status
/// </summary>
internal class GameIntentionConverterException : Exception
{
    /// <inheritdoc />
    public GameIntentionConverterException(GameStatus gameStatus)
        : base($"Game intention for status {gameStatus} is not defined")
    {
    }
}