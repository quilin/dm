using System;
using DM.Services.Core.Dto.Enums;

namespace DM.Services.Gaming.BusinessProcesses.Characters.Updating;

/// <summary>
/// Argument out of range exception for game status
/// </summary>
internal class CharacterIntentionConverterException : Exception
{
    /// <inheritdoc />
    public CharacterIntentionConverterException(CharacterStatus characterStatus)
        : base($"Character intention for status {characterStatus} is not defined")
    {
    }
}