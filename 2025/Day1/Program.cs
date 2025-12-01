// Advent of Code challenge: https://adventofcode.com/2025/day/1

using AoC.Shared.Strings;

Console.WriteLine("AoC - Day 1\n\n");

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    Console.WriteLine($"[{inputFile}]\n");

    var rotations = File.ReadAllLines(inputFile)
        .Where(line => !string.IsNullOrEmpty(line))
        .Select(line => (dir: line[0], dist: line[1..].ToInt32()))
        .ToList();

    var dial = 50;
    var part1 = 0;
    var part2 = 0;

    foreach (var (dir, dist) in rotations)
    {
        var toZero = dir == 'R' ? 100 - dial: dial;
        if (toZero == 0)
        {
            toZero = 100;
        }

        if (dist >= toZero)
        {
            part2 += 1 + (dist - toZero) / 100;
        }
        
        dial += dir == 'R' ? dist : -dist;
        dial = (dial % 100 + 100) % 100;
        
        if (dial == 0)
        {
            part1 += 1;
        }
    }
    
    Console.WriteLine($"Part 1: {part1}");
    Console.WriteLine($"Part 2: {part2}\n");
}
