using SomeRSet.Game;
using SomeRSet.Services;

namespace SomeRSet;

internal class Program
{
    static void Main(string[] args)
    {
        var engine = new GameEngine();
        var renderer = new RenderService();
        var input = new InputService();

        engine.Initialize(seed: 12345);
        renderer.Render(engine.State);

        while (true)
        {
            var raw = input.ReadLine();
            var cmd = input.Parse(raw);

            switch (cmd.Type)
            {
                case CommandType.Help:
                    Console.WriteLine("Commands: help, show, reshuffle <seed>, play <cardIndex>, quit");
                    break;

                case CommandType.Show:
                    renderer.Render(engine.State);
                    break;

                case CommandType.Reshuffle:
                    engine.Initialize(cmd.Seed!.Value);
                    renderer.Render(engine.State);
                    break;

                case CommandType.Play:
                    if (cmd.CardIndex is null)
                    {
                        Console.WriteLine("Missing card index.");
                        break;
                    }

                    var move = new SomeRSet.Engine.Move(SomeRSet.Engine.MoveType.PlayCard, cmd.CardIndex.Value);
                    var (ok, err) = engine.TryApplyMove(move);
                    if (!ok)
                        Console.WriteLine(err);
                    else
                        renderer.Render(engine.State);
                    break;

                case CommandType.Quit:
                    return;

                default:
                    Console.WriteLine(cmd.Error ?? "Unknown command. Type 'help'.");
                    break;
            }
        }
    }
}