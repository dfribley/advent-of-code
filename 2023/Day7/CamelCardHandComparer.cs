namespace Day7;

public class CamelCardHandComparer : IComparer<(string hand, HandType type, int bid)>
{
    private readonly string _cardsInOrder;

    public CamelCardHandComparer(string cardsInOrder)
    {
        _cardsInOrder = cardsInOrder;
    }

    public int Compare((string hand, HandType type, int bid) x, (string hand, HandType type, int bid) y)
    {
        if (x.type < y.type)
        {
            return -1;
        }

        if (x.type > y.type)
        {
            return 1;
        }

        foreach (var i in Enumerable.Range(0, 5))
        {
            if (x.hand[i] != y.hand[i])
            {
                var xval = _cardsInOrder.IndexOf(x.hand[i]);
                var yval = _cardsInOrder.IndexOf(y.hand[i]);

                return xval < yval
                    ? -1
                    : xval > yval
                        ? 1
                        : 0;
            }
        }

        return 0;
    }
}
