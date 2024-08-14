namespace DM.Services.Core.Dto.Enums;

/// <summary>
/// Attribute schema access type
/// </summary>
public enum SchemaType
{
    /// <summary>
    /// Everyone can use, nobody can edit
    /// </summary>
    Public = 0,

    /// <summary>
    /// Only author can use and edit
    /// </summary>
    Private = 1
}