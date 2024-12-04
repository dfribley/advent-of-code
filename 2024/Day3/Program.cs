// Advent of Code challenge: https://adventofcode.com/2024/day/3

using System.Text.RegularExpressions;
using AoC.Shared.Strings;

Console.WriteLine("AoC - Day 3\n\n");

var regex = new Regex(@"mul\((\d{1,3}),(\d{1,3})\)|do\(\)|don't\(\)");

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    Console.WriteLine($"[{inputFile}]\n");

    var input = File.ReadAllText(inputFile);
    var matches = regex.Matches(input);

    var part1 = matches
        .Where(m => m.Value.StartsWith('m'))
        .Select(m => m.Groups[1].Value.ToInt32() * m.Groups[2].Value.ToInt32())
        .Sum();
    
    Console.WriteLine($"Part 1: {part1}");

    var part2 = 0;
    var enabled = true;
    foreach (Match match in matches)
    {
        if (match.Value.StartsWith('d'))
        {
            enabled = !match.Value.Contains('\'');
            continue;
        }

        if (enabled)
        {
            part2 += match.Groups[1].Value.ToInt32() * match.Groups[2].Value.ToInt32();
        }
    }
    
    Console.WriteLine($"Part 2: {part2}\n");
}
