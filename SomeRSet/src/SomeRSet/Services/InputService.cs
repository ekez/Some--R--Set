namespace SomeRSet.Services;

public sealed class InputService
{
    public string ReadLine()
    {
        Console.Write("> ");
        return Console.ReadLine() ?? "";
    }

    public Command Parse(string raw)
    {
        if (string.IsNullOrWhiteSpace(raw))
            return new Command(CommandType.Unknown, Error: "Empty command. Type 'help'.");

        var parts = raw.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var verb = parts[0].ToLowerInvariant();

        return verb switch
        {
            "help" => new Command(CommandType.Help),
            "show" => new Command(CommandType.Show),
            "quit" => new Command(CommandType.Quit),
            "exit" => new Command(CommandType.Quit),
            "reshuffle" => ParseReshuffle(parts),
            "play" => ParsePlay(parts),
            _ => new Command(CommandType.Unknown, Error: $"Unknown command '{parts[0]}'. Type 'help'.")
        };
    }

    private static Command ParseReshuffle(string[] parts)
    {
        if (parts.Length != 2)
            return new Command(CommandType.Unknown, Error: "Usage: reshuffle <seed>");

        if (!int.TryParse(parts[1], out var seed))
            return new Command(CommandType.Unknown, Error: "Seed must be an integer. Usage: reshuffle <seed>");

        return new Command(CommandType.Reshuffle, Seed: seed);
    }

    private static Command ParsePlay(string[] parts)
    {
        if (parts.Length != 2)
            return new Command(CommandType.Unknown, Error: "Usage: play <cardIndex>");

        if (!int.TryParse(parts[1], out var idx))
            return new Command(CommandType.Unknown, Error: "Card index must be an integer. Usage: play <cardIndex>");

        return new Command(CommandType.Play, CardIndex: idx);
    }
}