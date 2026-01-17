namespace SomeRSet.Engine;

public enum MoveType { PlayCard }

public sealed record Move(MoveType Type, int CardIndex);
