using System;

namespace SomeRSet.Providers
{
    public class RandomProvider : IRandomProvider
    {
        private readonly Random _rng = new();
        public int Next(int minValue, int maxValue) => _rng.Next(minValue, maxValue);
    }
}
