namespace SomeRSet.Services;

public enum CommandType
{
    Help,
    Show,
    Reshuffle,
    Play,
    Quit,
    Unknown
}

public sealed record Command(CommandType Type, int? Seed = null, int? CardIndex = null, string? Error = null);