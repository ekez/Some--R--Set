namespace SomeRSet.Engine;

public sealed class Player
{
    public string Name { get; }
    public Hand Hand { get; } = new();

    public Player(string name) => Name = name;
}
