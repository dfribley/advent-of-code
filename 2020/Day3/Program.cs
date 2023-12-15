using System.Drawing;
using AoC.Shared.Enumerable;
using AoC.Shared.Grid;
using AoC.Shared.Points;

Console.WriteLine("AOC - Day 3\n\n");

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    Console.WriteLine($"[{inputFile}]\n");

    var field = File.ReadAllLines(inputFile)
        .Where(line => !string.IsNullOrEmpty(line))
        .ToGrid();
    field.WrapEast = true;

    long totalTrees(Point slope)
    {
        var toboggan = field.TopLeft;
        var trees = 0;

        while (field.IsValid(toboggan))
        {
            if (field[toboggan] == '#')
            {
                trees++;
            }

            toboggan = toboggan.Add(slope);
        }

        return trees;
    }

    Console.WriteLine($"Part 1: { totalTrees(new Point(3, -1)) } ");

    var slopes = new[]
    {
        new Point(1, -1),
        new Point(3, -1),
        new Point(5, -1),
        new Point(7, -1),
        new Point(1, -2)
    };

    Console.WriteLine($"Part 2: {slopes.Select(totalTrees).Product()}\n");
}
