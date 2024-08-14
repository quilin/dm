using System;
using System.ComponentModel;
using System.Reflection;

namespace DM.Services.Core.Extensions;

/// <summary>
/// Расширения аттрибута описания
/// </summary>
public static class DescriptionExtensions
{
    /// <summary>
    /// Получить значение <see cref="DescriptionAttribute" /> для текущего значения перечисления
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string GetDescription(this Enum value) =>
        value.GetType().GetField(value.ToString())
            ?.GetCustomAttribute<DescriptionAttribute>()
            ?.Description;
}