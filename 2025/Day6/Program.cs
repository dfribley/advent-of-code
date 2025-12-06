// Advent of Code challenge: https://adventofcode.com/2025/day/6

using AoC.Shared.Enumerable;
using AoC.Shared.Grid;

Console.WriteLine("AoC - Day 6\n\n");

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    Console.WriteLine($"[{inputFile}]\n");

    var input = File.ReadAllLines(inputFile).ToList();
    var ub = input.First().Length;
    var problems = input
        .Last()
        .Select((c, i) => (c, i))
        .Where(t => t.c != ' ')
        .Reverse()
        .Select(t =>
        {
            var grid = new Grid(input
                .Take(input.Count - 1)
                .Select(line => line.Substring(t.i, ub - t.i))
            );

            ub = t.i - 1;
            return (op: t.c, grid);
        })
        .ToList();
    
    Console.WriteLine($"Part 1: {GrandTotal()}");

    problems = problems
        .Select(t => (t.op, grid: t.grid.Rotate90Prime()))
        .ToList();
    
    Console.WriteLine($"Part 2: {GrandTotal()}\n");
    
    continue;

    long GrandTotal()
    {
        return problems
            .Select(t => t.op == '+' 
                ? t.grid.rows.Select(long.Parse).Sum() 
                : t.grid.rows.Select(long.Parse).Product())
            .Sum();
    }
}
