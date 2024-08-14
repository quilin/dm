using System.ComponentModel;

namespace DM.Services.Core.Dto.Enums;

/// <summary>
/// Website color schema
/// </summary>
public enum ColorSchema
{
    /// <summary>
    /// Base DM3 color schema
    /// </summary>
    [Description("Новая")]
    Modern = 0,

    /// <summary>
    /// DM3 color schema with paler colors
    /// </summary>
    [Description("Новая контрастная")]
    Pale = 1,

    /// <summary>
    /// Base DM2 color schema
    /// </summary>
    [Description("Классическая")]
    Classic = 2,

    /// <summary>
    /// DM2 color schema with paler colors
    /// </summary>
    [Description("Классическая контрастная")]
    ClassicPale = 3,

    /// <summary>
    /// Nightly mode
    /// </summary>
    [Description("Ночная")]
    Night = 4
}