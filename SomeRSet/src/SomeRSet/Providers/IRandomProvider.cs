namespace SomeRSet.Providers;

public interface IRandomProvider
{
    int NextInt(int minInclusive, int maxExclusive);
}
