// Advent of Code challenge: https://adventofcode.com/2025/day/7

using System.Drawing;
using AoC.Shared.Grid;
using AoC.Shared.Points;

Console.WriteLine("AoC - Day 7\n\n");

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    Console.WriteLine($"[{inputFile}]\n");

    var diagram = File.ReadAllLines(inputFile)
        .Where(line => !string.IsNullOrEmpty(line))
        .ToGrid();

    var beams = new Dictionary<Point, long> { { diagram.First(c => c == 'S'), 1 } };
    var splits = 0;
    
    for (var y = diagram.MaxY; y > 0; y--)
    {
        var newBeams = new Dictionary<Point, long>();
        
        foreach (var kvp in beams)
        {
            var next = kvp.Key.Add(GridDirections.South);

            if (diagram[next] == '^')
            {
                splits++;

                foreach (var neighbor in GridDirections.Horizontal.Select(dir => next.Add(dir)))
                {
                    newBeams[neighbor] = newBeams.GetValueOrDefault(neighbor) + kvp.Value;
                }
            }
            else
            {
                newBeams[next] = newBeams.GetValueOrDefault(next) + kvp.Value;
            }
        }
        
        beams = newBeams;
    }
    
    Console.WriteLine($"Part 1: {splits}");
    Console.WriteLine($"Part 2: {beams.Values.Sum()}\n");
}
