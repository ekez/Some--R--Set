using SomeRSet.Game;

namespace SomeRSet.Services;

public sealed class RenderService
{
    public void Render(GameState state)
    {
        Console.WriteLine($"Current player: {state.CurrentPlayerIndex} ({state.Players[state.CurrentPlayerIndex].Name})");
        if (state.CurrentTrick.Count > 0)
        {
            Console.WriteLine("Current trick:");
            foreach (var entry in state.CurrentTrick)
            {
                Console.WriteLine($"  P{entry.PlayerIndex}: {entry.Card}");
            }
            Console.WriteLine();
        }

        Console.WriteLine($"Seed: {state.Seed}");
        Console.WriteLine($"Deck remaining: {state.RemainingDeck.Count}");
        Console.WriteLine();

        foreach (var p in state.Players)
        {
            Console.WriteLine($"{p.Name} hand ({p.Hand.Cards.Count}):");
            for (var i = 0; i < p.Hand.Cards.Count; i++)
                Console.WriteLine($"  [{i}] {p.Hand.Cards[i]}");
            Console.WriteLine();
        }
    }
}