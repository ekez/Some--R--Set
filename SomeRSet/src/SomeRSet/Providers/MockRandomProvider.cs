namespace SomeRSet.Providers;

public sealed class MockRandomProvider : IRandomProvider
{
    private readonly Queue<int> values;

    public MockRandomProvider(IEnumerable<int> values)
    {
        this.values = new Queue<int>(values);
    }

    public int NextInt(int minInclusive, int maxExclusive)
    {
        if (values.Count == 0)
            throw new InvalidOperationException("MockRandomProvider ran out of values");

        var value = values.Dequeue();

        if (value < minInclusive || value >= maxExclusive)
            throw new ArgumentOutOfRangeException(
                $"Mock value {value} outside range [{minInclusive}, {maxExclusive})");

        return value;
    }
}