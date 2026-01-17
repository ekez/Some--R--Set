using System.Collections.Generic;
using SomeRSet.Engine;

namespace SomeRSet.Game;

public sealed class GameState
{
    public int Seed { get; init; }
    public List<Player> Players { get; init; } = new();
    public List<Card> RemainingDeck { get; init; } = new();

    // Turn/trick state
    public int CurrentPlayerIndex { get; set; }
    public int TrumpDenominator { get; set; } = 6;

    public int? LeadDenominator { get; set; }
    public List<(int PlayerIndex, Card Card)> CurrentTrick { get; } = new();

    public int? LastTrickWinner { get; set; }
}
