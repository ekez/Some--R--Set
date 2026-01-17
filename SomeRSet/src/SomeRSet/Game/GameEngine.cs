using System.Collections.Generic;
using System.Linq;
using SomeRSet.Engine;
using SomeRSet.Providers;

namespace SomeRSet.Game;

public sealed class GameEngine
{
    public GameState State { get; private set; } = new GameState();

    public void Initialize(int seed)
    {
        var rng = new RandomProvider(seed);

        var deck = Deck.CreateStandardDeck();
        Deck.ShuffleInPlace(deck, rng);

        var players = new List<Player>
        {
            new Player("P1"),
            new Player("P2"),
            new Player("P3"),
            new Player("P4"),
        };

        foreach (var p in players)
        {
            for (var i = 0; i < 7; i++)
            {
                var card = deck[0];
                deck.RemoveAt(0);
                p.Hand.Cards.Add(card);
            }
        }

        State = new GameState
        {
            Seed = seed,
            Players = players,
            RemainingDeck = deck
        };
    }

    public (bool Success, string? Error) TryApplyMove(Move move)
    {
        if (move.Type != MoveType.PlayCard)
            return (false, "Unsupported move");

        if (State.Players is null || State.Players.Count == 0)
            return (false, "No players in game");

        var pIndex = State.CurrentPlayerIndex;
        if (pIndex < 0 || pIndex >= State.Players.Count)
            return (false, "Invalid current player index");

        var player = State.Players[pIndex];

        if (move.CardIndex < 0 || move.CardIndex >= player.Hand.Cards.Count)
            return (false, "Card index out of range");

        var card = player.Hand.Cards[move.CardIndex];

        // Enforce follow-suit: if lead is set and player has a card in that suit, they must play it
        if (State.LeadDenominator is int leadDen)
        {
            var hasLead = player.Hand.Cards.Any(c => c.Denominator == leadDen);
            if (hasLead && card.Denominator != leadDen)
                return (false, "Must follow lead suit");
        }

        // Apply move
        player.Hand.Cards.RemoveAt(move.CardIndex);

        if (State.CurrentTrick.Count == 0)
            State.LeadDenominator = card.Denominator;

        State.CurrentTrick.Add((pIndex, card));

        // Advance turn or resolve trick
        if (State.CurrentTrick.Count < State.Players.Count)
        {
            State.CurrentPlayerIndex = (State.CurrentPlayerIndex + 1) % State.Players.Count;
        }
        else
        {
            // Determine winner
            var winner = DetermineTrickWinner();
            State.LastTrickWinner = winner;
            State.CurrentTrick.Clear();
            State.LeadDenominator = null;
            State.CurrentPlayerIndex = winner;
        }

        return (true, null);
    }

    private int DetermineTrickWinner()
    {
        if (State.CurrentTrick.Count == 0)
            return State.CurrentPlayerIndex;

        var leadDen = State.LeadDenominator ?? State.CurrentTrick[0].Card.Denominator;
        var trumpDen = State.TrumpDenominator;

        var best = State.CurrentTrick[0];
        foreach (var entry in State.CurrentTrick.Skip(1))
        {
            if (Beats(entry.Card, best.Card, leadDen, trumpDen))
                best = entry;
        }

        return best.PlayerIndex;
    }

    private static bool Beats(Card a, Card b, int leadDen, int trumpDen)
    {
        var aIsTrump = a.Denominator == trumpDen;
        var bIsTrump = b.Denominator == trumpDen;

        if (aIsTrump && !bIsTrump)
            return true;
        if (!aIsTrump && bIsTrump)
            return false;

        if (aIsTrump && bIsTrump)
            return a.Numerator > b.Numerator;

        var aIsLead = a.Denominator == leadDen;
        var bIsLead = b.Denominator == leadDen;

        if (aIsLead && !bIsLead)
            return true;
        if (!aIsLead && bIsLead)
            return false;

        if (aIsLead && bIsLead)
            return a.Numerator > b.Numerator;

        return false;
    }
}
