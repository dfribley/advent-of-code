using Shared.Enumerable;

Console.WriteLine("AOC - Day 14\n\n");

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    Console.WriteLine($"[{inputFile}]\n");

    var input = File.ReadAllLines(inputFile)
        .Split(string.IsNullOrEmpty)
        .ToList();
    var template = input[0].Values.First();
    var rules = input[1].Values
        .Select(line =>
        {
            var parts = line.Split(" -> ");

            return (pattern: parts[0], insert: parts[1]);
        })
        .ToDictionary(t => t.pattern, t => t.insert);

    IEnumerable<long> getCounts(string template, int steps)
    {
        var counts = new Dictionary<string, long>();

        for (var i = 0; i < template.Length - 1; i++)
        {
            var pair = template.Substring(i, 2);

            counts.TryAdd(pair, 0);
            counts[pair]++;
        }

        while (steps-- > 0)
        {
            var newCounts = new Dictionary<string, long>();

            foreach (var pair in counts.Keys)
            {
                var insert = rules[pair];

                foreach(var newPair in new[] { $"{pair[0]}{insert}", $"{insert}{pair[1]}" })
                {
                    newCounts.TryAdd(newPair, 0);
                    newCounts[newPair] += counts[pair];
                }
            }

            counts = newCounts;
        }

        var characterCounts = counts.Keys
            .Select(k => (character: k[0], count: counts[k]))
            .GroupBy(t => t.character)
            .ToDictionary(g => g.Key, g => g.Sum(t => t.count));

        characterCounts.TryAdd(template.Last(), 0);
        characterCounts[template.Last()]++;

        return characterCounts.Values.Order().ToArray();
    }

    var counts10 = getCounts(template, 10);
    Console.WriteLine($"Part 1: {counts10.Last() - counts10.First()}");

    var counts40 = getCounts(template, 40);
    Console.WriteLine($"Part 2: {counts40.Last() - counts40.First()}\n");
}