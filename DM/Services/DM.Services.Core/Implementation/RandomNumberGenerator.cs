using System;

namespace DM.Services.Core.Implementation
{
    /// <inheritdoc />
    public class RandomNumberGenerator : IRandomNumberGenerator
    {
        private static readonly Lazy<Random> Random = new Lazy<Random>(() => new Random());

        /// <inheritdoc />
        public int Generate(int minValue, int maxValue) => Random.Value.Next(minValue, maxValue + 1);

        /// <inheritdoc />
        public int Generate(int maxValue) => Generate(1, maxValue);
    }
}