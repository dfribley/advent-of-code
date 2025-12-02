// Advent of Code challenge: https://adventofcode.com/2019/day/10

using System.Drawing;
using AoC.Shared.Grid;
using AoC.Shared.Lines;
using AoC.Shared.Points;

Console.WriteLine("AoC - Day 10\n\n");

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    Console.WriteLine($"[{inputFile}]\n");

    var grid = File.ReadAllLines(inputFile)
        .Where(line => !string.IsNullOrEmpty(line))
        .ToGrid();

    var asteroids = grid
        .Where(p => p.value == '#')
        .Select(p => p.coordinate)
        .ToHashSet();
    var max = 0;

    foreach (var a in asteroids)
    {
        foreach (var b in asteroids.Where(b => a != b).ToList())
        {
            //if (asteroids.Where(c => a != c && b != c).ToList().Any())
        }
    }

    Console.WriteLine("Total Asteroids: " + asteroids.Count);
    Console.WriteLine($"Part 1: {max}");
    Console.WriteLine($"Part 2:\n");
}
