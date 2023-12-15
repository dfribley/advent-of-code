using AoC.Shared.Strings;

Console.WriteLine("AOC - Day 4\n\n");

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    Console.WriteLine($"[{inputFile}]\n");

    var cards = File.ReadAllLines(inputFile)
        .Where(line => !string.IsNullOrEmpty(line))
        .Select(card =>
        {
            var parts = card.Split(":");
            var id = parts[0].Split(" ", StringSplitOptions.RemoveEmptyEntries)[1].ToInt32();

            parts = parts[1].Split("|");
            var mine = parts[0].Split(" ", StringSplitOptions.RemoveEmptyEntries);
            var winning = parts[1].Split(" ", StringSplitOptions.RemoveEmptyEntries);

            return (id, matches: mine.Where(winning.Contains).Count());
        })
    .ToList();

    var part1 = cards
        .Select(c => c.matches > 0 ? Math.Pow(2, c.matches - 1) : 0)
        .Sum();

    Console.WriteLine($"Part 1: {part1}");

    var cardCounts = cards.ToDictionary(c => c.id, c => 1);

    cards.ForEach(card =>
    {
        while (card.matches > 0)
        {
            var cloneId = card.id + card.matches--;

            if (cardCounts.ContainsKey(cloneId))
            {
                cardCounts[cloneId] += cardCounts[card.id];
            }
        }
    });

    Console.WriteLine($"Part 2: {cardCounts.Values.Sum()}\n");
}
