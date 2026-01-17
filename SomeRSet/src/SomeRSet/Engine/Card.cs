namespace SomeRSet.Engine;

public sealed record Card(
    int Id,
    int Numerator,
    int Denominator,
    bool IsRedCounter,
    int CounterPoints
)
{
    public override string ToString()
        => $"{Numerator}/{Denominator}" + (IsRedCounter ? $" (red {CounterPoints})" : "");
}
