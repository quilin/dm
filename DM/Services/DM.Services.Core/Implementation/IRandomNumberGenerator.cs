namespace DM.Services.Core.Implementation
{
    public interface IRandomNumberGenerator
    {
        int Generate(int minValue, int maxValue);
        int Generate(int maxValue);
    }
}