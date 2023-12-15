using AoC.Shared.Strings;
using Day7;

// Custom Sorting
Console.WriteLine("AOC - Day 7\n\n");

var cards = "23456789TJQKA";

HandType handType(string hand)
{
    if (cards.Any(c => hand.Where(hc => hc == c).Count() == 5))
        return HandType.FiveOfAKind;

    if (cards.Any(c => hand.Where(hc => hc == c).Count() == 4))
        return HandType.FourOfAKind;

    if (hand.Distinct().Count() == 2)
        return HandType.FullHouse;

    if (cards.Any(c => hand.Where(hc => hc == c).Count() == 3))
        return HandType.ThreeOfAKind;

    if (cards.Count(c => hand.Count(hc => hc == c) == 2) == 2)
        return HandType.TwoPair;

    if (cards.Any(c => hand.Count(hc => hc == c) == 2))
        return HandType.OnePair;

    return HandType.HighCard;
}

int winnings(IEnumerable<(string hand, HandType type, int bid)> hands)
{
    return hands
        .Select((hand, i) => hand.bid * (i + 1))
        .Sum();
}

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    Console.WriteLine($"[{inputFile}]\n");

    var input = File.ReadAllLines(inputFile)
        .Where(line => !string.IsNullOrEmpty(line));

    var hands = File.ReadAllLines(inputFile)
        .Where(line => !string.IsNullOrEmpty(line))
        .Select(line =>
        {
            var parts = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            var hand = parts[0];

            return (hand, type: handType(hand), bid: parts[1].ToInt32());
        })
        .ToList();

    hands.Sort(new CamelCardHandComparer(cards));

    Console.WriteLine($"Part 1: {winnings(hands)}");

    hands = hands
        .Select(t =>
        {
            var types = new List<HandType>() { t.type };

            foreach(var card in cards)
            {
                var newHand = t.hand.Replace('J', card);

                if (newHand != t.hand)
                {
                    types.Add(handType(newHand));
                }
            }

            return (t.hand, types.OrderDescending().First(), t.bid);
        })
    .ToList();

    hands.Sort(new CamelCardHandComparer(cards.Replace("J", "").Insert(0, "J")));

    Console.WriteLine($"Part 2: {winnings(hands)}\n");
}
