namespace SomeRSet.Engine;

public static class Deck
{
    public static List<Card> CreateStandardDeck()
    {
        var cards = new List<Card>(28);
        var id = 0;

        for (var d = 0; d <= 6; d++)
        {
            for (var n = 0; n <= d; n++)
            {
                var (isRed, points) = GetCounterInfo(n, d);
                cards.Add(new Card(id++, n, d, isRed, points));
            }
        }

        return cards;
    }

    private static (bool isRed, int points) GetCounterInfo(int n, int d)
    {
        // Red counters:
        // 3/5 and 4/6 => 2 points
        // 1/2, 1/3, 2/4 => 1 point
        if (n == 3 && d == 5) return (true, 2);
        if (n == 4 && d == 6) return (true, 2);

        if (n == 1 && d == 2) return (true, 1);
        if (n == 1 && d == 3) return (true, 1);
        if (n == 2 && d == 4) return (true, 1);

        return (false, 0);
    }

    public static void ShuffleInPlace(List<Card> cards, Providers.IRandomProvider rng)
    {
        // Fisher-Yates
        for (var i = cards.Count - 1; i > 0; i--)
        {
            var j = rng.NextInt(0, i + 1);
            (cards[i], cards[j]) = (cards[j], cards[i]);
        }
    }
}
