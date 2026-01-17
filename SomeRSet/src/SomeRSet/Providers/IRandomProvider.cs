namespace SomeRSet.Providers
{
    public interface IRandomProvider
    {
        int Next(int minValue, int maxValue);
    }
}
