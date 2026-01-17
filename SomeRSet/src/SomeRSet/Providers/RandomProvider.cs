namespace SomeRSet.Providers;

public sealed class RandomProvider : IRandomProvider
{
    private readonly Random random;

    public RandomProvider(int seed) => random = new Random(seed);

    public int NextInt(int minInclusive, int maxExclusive)
        => random.Next(minInclusive, maxExclusive);
}