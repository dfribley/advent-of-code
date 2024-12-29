// Advent of Code challenge: https://adventofcode.com/2024/day/25

using AoC.Shared.Enumerable;
using AoC.Shared.Grid;

Console.WriteLine("AoC - Day 25\n\n");

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

    var locks = new List<Grid>();
    var keys = new List<Grid>();
    
    foreach (var gridGroup in input)
    {
        var grid = gridGroup.Values.ToGrid();

        if (grid.GetRow(grid.MaxY).All(c => c == '#'))
        {
            locks.Add(grid);
        }
        else
        {
            keys.Add(grid);
        }
    }

    var part1 = (
            from lck in locks
            from key in keys
            select key.Where(chr => chr == '#').All(xy => lck[xy] == '.'))
        .Count(match => match);

    Console.WriteLine($"Part 1: {part1}\n");
}
