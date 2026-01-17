using SomeRSet.Engine;
using SomeRSet.Game;
using Xunit;

namespace SomeRSet.Tests;

public sealed class GameEngineTests
{
    [Fact]
    public void FollowSuit_RejectsOffSuitWhenPlayerHasLead()
    {
        var engine = new GameEngine();
        engine.Initialize(seed: 1);

        // Prepare players
        engine.State.Players.Clear();
        for (var i = 0; i < 4; i++)
            engine.State.Players.Add(new Player($"P{i}"));

        engine.State.CurrentPlayerIndex = 0;
        engine.State.LeadDenominator = 4;

        var p0 = engine.State.Players[0];
        p0.Hand.Cards.Clear();
        p0.Hand.Cards.Add(new Card(1, 1, 4, false, 0)); // lead suit
        p0.Hand.Cards.Add(new Card(2, 2, 5, false, 0)); // off-suit

        var move = new Move(MoveType.PlayCard, 1); // try to play off-suit
        var (ok, err) = engine.TryApplyMove(move);
        Assert.False(ok);
        Assert.Equal("Must follow lead suit", err);
    }

    [Fact]
    public void FollowSuit_AllowsOffSuitWhenPlayerLacksLead()
    {
        var engine = new GameEngine();
        engine.Initialize(seed: 1);

        engine.State.Players.Clear();
        for (var i = 0; i < 4; i++)
            engine.State.Players.Add(new Player($"P{i}"));

        engine.State.CurrentPlayerIndex = 0;
        engine.State.LeadDenominator = 4;

        var p0 = engine.State.Players[0];
        p0.Hand.Cards.Clear();
        p0.Hand.Cards.Add(new Card(10, 3, 5, false, 0)); // no lead suit in hand

        var move = new Move(MoveType.PlayCard, 0);
        var (ok, err) = engine.TryApplyMove(move);
        Assert.True(ok, err);
        Assert.Equal(1, engine.State.CurrentTrick.Count);
        Assert.Equal(10, engine.State.CurrentTrick[0].Card.Id);
    }

    [Fact]
    public void TrickWinner_TrumpBeatsLead()
    {
        var engine = new GameEngine();
        engine.Initialize(seed: 1);

        engine.State.Players.Clear();
        for (var i = 0; i < 4; i++)
            engine.State.Players.Add(new Player($"P{i}"));

        engine.State.CurrentPlayerIndex = 0;
        // default trump is 6

        // Player0 leads with denom 5
        var p0 = engine.State.Players[0];
        p0.Hand.Cards.Clear();
        p0.Hand.Cards.Add(new Card(1, 2, 5, false, 0));

        // Player1 follows lead denom 5
        var p1 = engine.State.Players[1];
        p1.Hand.Cards.Clear();
        p1.Hand.Cards.Add(new Card(2, 3, 5, false, 0));

        // Player2 plays trump denom 6
        var p2 = engine.State.Players[2];
        p2.Hand.Cards.Clear();
        p2.Hand.Cards.Add(new Card(3, 1, 6, false, 0));

        // Player3 follows lead denom 5
        var p3 = engine.State.Players[3];
        p3.Hand.Cards.Clear();
        p3.Hand.Cards.Add(new Card(4, 4, 5, false, 0));

        // Play four cards in order
        Assert.True(engine.TryApplyMove(new Move(MoveType.PlayCard, 0)).Success);
        Assert.True(engine.TryApplyMove(new Move(MoveType.PlayCard, 0)).Success);
        Assert.True(engine.TryApplyMove(new Move(MoveType.PlayCard, 0)).Success);
        var result = engine.TryApplyMove(new Move(MoveType.PlayCard, 0));
        Assert.True(result.Success, result.Error);

        // Player2 (index 2) played the trump and should win
        Assert.Equal(2, engine.State.LastTrickWinner);
        Assert.Equal(2, engine.State.CurrentPlayerIndex);
        Assert.Empty(engine.State.CurrentTrick);
        Assert.Null(engine.State.LeadDenominator);
    }
}
