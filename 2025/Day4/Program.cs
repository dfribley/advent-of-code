// Advent of Code challenge: https://adventofcode.com/2025/day/4

using AoC.Shared.Grid;
using AoC.Shared.Points;

Console.WriteLine("AoC - Day 4\n\n");

const char paperChar = '@';

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

    var part1 = grid
        .Where(c => c == paperChar)
        .Count(p => GridDirections.Neighbors
            .Select(dir => p.Add(dir))
            .Where(grid.IsValid)
            .Count(n => grid[n] == paperChar) < 4);

    Console.WriteLine($"Part 1: {part1}");
    
    var removed = 0;
    while (true)
    {
        var toRemove = grid
            .Where(c => c == paperChar)
            .Where(p => GridDirections.Neighbors
                .Select(dir => p.Add(dir))
                .Where(grid.IsValid)
                .Count(n => grid[n] == paperChar) < 4)
            .ToList();
        
        if (toRemove.Count == 0)
        {
            break;
        }
        
        removed += toRemove.Count;
        foreach (var paper in toRemove)
        {
            grid[paper] = '.';
        }
    }
    
    Console.WriteLine($"Part 2: {removed}\n");
}
