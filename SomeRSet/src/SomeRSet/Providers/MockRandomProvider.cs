namespace SomeRSet.Providers
{
    public class MockRandomProvider : IRandomProvider
    {
        private readonly int _fixed;
        public MockRandomProvider(int fixedValue = 0) => _fixed = fixedValue;
        public int Next(int minValue, int maxValue) => _fixed;
    }
}
