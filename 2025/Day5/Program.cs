// Advent of Code challenge: https://adventofcode.com/2025/day/5

using AoC.Shared.Enumerable;
using AoC.Shared.Ranges;

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
        .ToList();

    var ranges = input[0]
        .Values
        .Select(Range64.Parse)
        .ToList();

    var ingredients = input[1]
        .Values
        .Select(long.Parse)
        .ToList();

    var part1 = ingredients
        .Count(ingredient => ranges.Any(r => r.Contains(ingredient)));

    Console.WriteLine($"Part 1: {part1}");
    
    for (var i = 0; i < ranges.Count - 1; i++)
    {
        var range = ranges[i];

        for (var r2 = i + 1; r2 < ranges.Count; r2++)
        {
            var range2 = ranges[r2];
            
            if (range2.FullyContains(range))
            {
                ranges.RemoveAt(i);
                i--;
                break;
            }
            
            if (range.FullyContains(range2))
            {
                ranges.RemoveAt(r2);
                r2--;
                continue;
            }
            
            range.ShrinkToExclude(range2);
        }
    }
    
    var part2 = ranges
        .Sum(r => r.Size);
    
    Console.WriteLine($"Part 2: {part2}\n");
}
