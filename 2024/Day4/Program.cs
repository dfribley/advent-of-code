// Advent of Code challenge: https://adventofcode.com/2024/day/4

using AoC.Shared.Grid;
using AoC.Shared.Points;

Console.WriteLine("AoC - Day 4\n\n");

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    Console.WriteLine($"[{inputFile}]\n");

    var puzzle = File.ReadAllLines(inputFile)
        .Where(line => !string.IsNullOrEmpty(line))
        .ToGrid();

    const string p1Word = "XMAS";

    var part1 = puzzle
        .Select(ltr => GridDirections.Neighbors.Count(dir => puzzle.Search(ltr.coordinate, dir, p1Word)))
        .Sum();
    
    Console.WriteLine($"Part 1: {part1}");
    
    const string p2Word = "MAS";
    const string p2WordReversed = "SAM";

    var part2 = puzzle
        .Count(ltr => 
            ltr.value == p2Word[1] &&
            puzzle.Search(ltr.coordinate.Add(GridDirections.NorthWest), GridDirections.SouthEast, p2Word, p2WordReversed) &&
            puzzle.Search(ltr.coordinate.Add(GridDirections.NorthEast), GridDirections.SouthWest, p2Word, p2WordReversed));

    Console.WriteLine($"Part 2: {part2}\n");
}
