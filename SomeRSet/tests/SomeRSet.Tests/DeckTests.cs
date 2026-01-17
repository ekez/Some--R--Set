using SomeRSet.Engine;
using SomeRSet.Providers;
using System.Linq;
using Xunit;

namespace SomeRSet.Tests;

public sealed class DeckTests
{
    [Fact]
    public void CreateStandardDeck_Has28UniqueCards()
    {
        var deck = Deck.CreateStandardDeck();

        Assert.Equal(28, deck.Count);
        Assert.Equal(28, deck.Select(c => c.Id).Distinct().Count());
    }

    [Fact]
    public void CreateStandardDeck_HasFiveRedCountersWorthSevenPointsTotal()
    {
        var deck = Deck.CreateStandardDeck();

        var reds = deck.Where(c => c.IsRedCounter).ToList();
        Assert.Equal(5, reds.Count);
        Assert.Equal(7, reds.Sum(c => c.CounterPoints));

        Assert.Contains(reds, c => c.Numerator == 3 && c.Denominator == 5 && c.CounterPoints == 2);
        Assert.Contains(reds, c => c.Numerator == 4 && c.Denominator == 6 && c.CounterPoints == 2);
        Assert.Contains(reds, c => c.Numerator == 1 && c.Denominator == 2 && c.CounterPoints == 1);
        Assert.Contains(reds, c => c.Numerator == 1 && c.Denominator == 3 && c.CounterPoints == 1);
        Assert.Contains(reds, c => c.Numerator == 2 && c.Denominator == 4 && c.CounterPoints == 1);
    }

    [Fact]
    public void Shuffle_IsDeterministicWithSeed()
    {
        var deck1 = Deck.CreateStandardDeck();
        var deck2 = Deck.CreateStandardDeck();

        Deck.ShuffleInPlace(deck1, new RandomProvider(12345));
        Deck.ShuffleInPlace(deck2, new RandomProvider(12345));

        Assert.Equal(deck1.Select(c => c.Id), deck2.Select(c => c.Id));
    }
}
