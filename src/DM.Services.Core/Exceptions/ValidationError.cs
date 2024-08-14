namespace DM.Services.Core.Exceptions;

/// <summary>
/// Set of server validation errors
/// </summary>
public static class ValidationError
{
    /// <summary>
    /// Field must not be null or default or whitespace string
    /// </summary>
    public const string Empty = nameof(Empty);

    /// <summary>
    /// The field value is too short
    /// </summary>
    public const string Short = nameof(Short);

    /// <summary>
    /// The field value is too long
    /// </summary>
    public const string Long = nameof(Long);

    /// <summary>
    /// The field value is already taken and requires to be unique
    /// </summary>
    public const string Taken = nameof(Taken);

    /// <summary>
    /// Field value does not match some pattern, e.g. email or regex
    /// </summary>
    public const string Invalid = nameof(Invalid);
}