// Advent of Code challenge: https://adventofcode.com/2024/day/19

using AoC.Shared.Enumerable;

Console.WriteLine("AoC - Day 19\n\n");

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    Console.WriteLine($"[{inputFile}]\n");

    var input = File.ReadAllLines(inputFile)
        .Split(string.IsNullOrEmpty)
        .ToList();

    var towels = input[0].Values.First().Split(", ");
    var designs = input[1].Values.ToArray();

    var options = designs
        .Select(GetOptions)
        .ToArray();

    Console.WriteLine($"Part 1: {options.Count(opt => opt > 0)}");
    Console.WriteLine($"Part 2: {options.Sum()}\n");
    
    continue;

    long GetOptions(string design)
    {
        var seen = new Dictionary<int, long>();

        return GetOptionsFromIndex(0);

        long GetOptionsFromIndex(int i)
        {
            if (i == design.Length)
            {
                return 1;
            }

            if (seen.TryGetValue(i, out var opts))
            {
                return opts;
            }

            var ways = towels
                .Where(t => design[i..].StartsWith(t))
                .Sum(t => GetOptionsFromIndex(i + t.Length));

            seen[i] = ways;
            return ways;
        }
    }
}
