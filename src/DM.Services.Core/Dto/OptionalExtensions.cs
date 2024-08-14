namespace DM.Services.Core.Dto;

/// <summary>
/// Extensions for optional comparison
/// </summary>
public static class OptionalExtensions
{
    /// <summary>
    /// Checks if the optional field is expected to change
    /// </summary>
    /// <param name="optional"></param>
    /// <param name="againstValue"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static bool HasChanged<T>(this Optional<T> optional, T? againstValue) where T : struct
    {
        return optional != null && !optional.Value.Equals(againstValue);
    }
}