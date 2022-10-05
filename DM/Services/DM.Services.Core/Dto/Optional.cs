namespace DM.Services.Core.Dto;

/// <summary>
/// DTO for optional monad for nullable types
/// </summary>
public class Optional<TType>
    where TType : struct
{
    private Optional(TType? value)
    {
        Value = value;
    }

    /// <summary>
    /// Nullable value
    /// </summary>
    public TType? Value { get; }

    /// <summary>
    /// Create new optional with value
    /// </summary>
    /// <returns></returns>
    public static Optional<TType> WithValue(TType? value)
    {
        return new Optional<TType>(value);
    }
}