namespace DM.Services.Core.Implementation;

/// <summary>
/// Generates random integer numbers
/// </summary>
internal interface IRandomNumberGenerator
{
    /// <summary>
    /// Generate random integer number in range
    /// </summary>
    /// <param name="minValue">Minimal possible value</param>
    /// <param name="maxValue">Maximum possible value</param>
    /// <returns>Random number</returns>
    int Generate(int minValue, int maxValue);

    /// <summary>
    /// Generate random integer number in range from 1
    /// </summary>
    /// <param name="maxValue">Maximum possible value</param>
    /// <returns>Random number</returns>
    int Generate(int maxValue);
}