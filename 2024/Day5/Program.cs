// Advent of Code challenge: https://adventofcode.com/2024/day/5

using AoC.Shared.Enumerable;
using AoC.Shared.Strings;

Console.WriteLine("AoC - Day 5\n\n");

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    Console.WriteLine($"[{inputFile}]\n");

    var input = File.ReadAllLines(inputFile)
        .Split(string.IsNullOrEmpty)
        .ToArray();

    var rules = input[0].Values
        .Select(ln =>
        {
            var parts = ln.Split("|");
            return (leading: parts[0].ToInt32(), trailing: parts[1].ToInt32());
        })
        .ToArray();
    var updates = input[1].Values
        .Select(ln => ln.Split(",").Select(int.Parse).ToArray())
        .ToArray();

    var part1 = updates
        .Where(IsUpdateValid)
        .Select(update => update[update.Length / 2])
        .Sum();
    
    Console.WriteLine($"Part 1: {part1}");
    
    var pageComparer = new PageComparer(rules);
    var part2 = updates
        .Where(update => !IsUpdateValid(update))
        .Select(update => update.Order(pageComparer).ToArray())
        .Select(update => update[update.Length / 2])
        .Sum();

    Console.WriteLine($"Part 2: {part2}\n");
    
    continue;

    bool IsUpdateValid(int[] update)
    {
        for (var i = 0; i < update.Length - 1; i++)
        {
            for (var j = i + 1; j < update.Length; j++)
            {
                if (rules.Any(r => r.leading == update[j] && r.trailing == update[i]))
                {
                    return false;
                }
            }
        }

        return true;
    }
}

internal class PageComparer((int, int)[] rules) : IComparer<int>
{
    private readonly (int leading, int trailing)[] _rules = rules;

    public int Compare(int x, int y)
    {
        if (_rules.Any(r => r.leading == x && r.trailing == y))
        {
            return -1;
        }
        
        if (_rules.Any(r => r.leading == y && r.trailing == x))
        {
            return 1;
        }

        return 0;
    }
}
