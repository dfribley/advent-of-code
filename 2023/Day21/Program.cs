using System.Drawing;
using AoC.Shared.Grid;
using AoC.Shared.Points;
using static AoC.Shared.Grid.GridDirections;

Console.WriteLine("AOC - Day 21\n\n");

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
    field.WrapWest = true;
    field.WrapSouth = true;
    field.WrapNorth = true;

    var start = field.First(c => c == 'S');
    var spots = new HashSet<Point>();

    var steps = 64;

    var process = new HashSet<Point> { start };

    while (--steps >= 0)
    {
        var newProcess = new HashSet<Point>();

        foreach(var spot in process)
        {
            foreach(var move in SideNeighbors)
            {
                var next = spot.Add(move);

                if (field[next] != '#')
                {
                    newProcess.Add(next);
                }
            }
        }

        process = newProcess;
    }

    Console.WriteLine($"Part 1: {process.Count}");
    Console.WriteLine($"Part 2:\n");
}
