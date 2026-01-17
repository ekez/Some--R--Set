using SomeRSet.Game;

namespace SomeRSet.Services;

public sealed class RenderService
{
    public void Render(GameState state)
    {
        if (state.IsFinished)
        {
            Console.WriteLine("GAME OVER");
            if (!string.IsNullOrWhiteSpace(state.FinishReason))
                Console.WriteLine(state.FinishReason);
            Console.WriteLine();
        }

        var current = state.CurrentPlayerIndex;
        var currentName = state.Players[current].Name;

        Console.WriteLine();
        Console.WriteLine($"Turn: P{current} ({currentName})");
        Console.WriteLine($"Trump denominator: {state.TrumpDenominator}");

        if (state.LeadDenominator is int lead)
            Console.WriteLine($"Lead denominator: {lead}");

        if (state.LastTrickWinner is int w)
            Console.WriteLine($"Last trick winner: P{w} ({state.Players[w].Name})");

        if (state.CurrentTrick.Count > 0)
        {
            Console.WriteLine();
            Console.WriteLine("Current trick so far:");
            foreach (var entry in state.CurrentTrick)
                Console.WriteLine($"  {state.Players[entry.PlayerIndex].Name}: {entry.Card}");
        }

        Console.WriteLine();
        Console.WriteLine($"Seed: {state.Seed}");
        Console.WriteLine($"Deck remaining: {state.RemainingDeck.Count}");
        Console.WriteLine();

        // Other players summary
        for (var i = 0; i < state.Players.Count; i++)
        {
            if (i == current) continue;
            var p = state.Players[i];
            Console.WriteLine($"{p.Name}: {p.Hand.Cards.Count} cards");
        }

        Console.WriteLine();
        Console.WriteLine($"{currentName} hand ({state.Players[current].Hand.Cards.Count}) sorted:");
        var hand = state.Players[current].Hand.Cards;

        var sorted = hand
            .Select((card, originalIndex) => (card, originalIndex))
            .OrderBy(x => x.card.Denominator)
            .ThenBy(x => x.card.Numerator)
            .ToList();

        foreach (var item in sorted)
            Console.WriteLine($"  [play {item.originalIndex}] {item.card}");

        Console.WriteLine();
        Console.WriteLine("Enter: help | show | play <n> | reshuffle <seed> | quit");
        Console.WriteLine();
    }
}