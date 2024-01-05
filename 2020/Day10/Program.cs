using AoC.Shared.Enumerable;
using AoC.Shared.Looping;
using AoC.Shared.Strings;

Console.WriteLine("AOC - Day 10\n\n");

foreach (var inputFile in new[] { "sample.txt", "input.txt"})
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    Console.WriteLine($"[{inputFile}]\n");

    var raiting = 3;
    var adapters = File.ReadAllLines(inputFile)
        .Where(line => !string.IsNullOrEmpty(line))
        .Select(line => line.ToInt32())
        .Order()
        .ToList();

    adapters.Insert(0, 0);
    adapters.Add(adapters.Max() + raiting);

    var part1 = adapters
        .Skip(1)
        .Select((val, i) => val - adapters[i])
        .Where(val => val == 1 || val == 3)
        .GroupBy(val => val)
        .Select(grp => grp.Count())
        .Product();

    Console.WriteLine($"Part 1: {part1}");

    var known = new Dictionary<int, long>();

    long getCombinations(int index)
    {
        if (index == adapters.Count - 1)
        {
            return 1;
        }

        if (known.ContainsKey(index))
        {
            return known[index];
        }

        var combinations = 0L;

        raiting.Loop((i) =>
        {
            var next = index + i + 1;

            if (next < adapters.Count && adapters[next] - adapters[index] <= raiting)
            {
                combinations += getCombinations(next);
            }
        });

        known.Add(index, combinations);
        return combinations;
    }

    Console.WriteLine($"Part 2: {getCombinations(0)}\n");
}
