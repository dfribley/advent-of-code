// Advent of Code challenge: https://adventofcode.com/2024/day/8

using System.Drawing;
using AoC.Shared.Enumerable;
using AoC.Shared.Grid;
using AoC.Shared.Lines;
using AoC.Shared.Points;

Console.WriteLine("AoC - Day 8\n\n");

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    Console.WriteLine($"[{inputFile}]\n");

    var map = File.ReadAllLines(inputFile)
        .Where(line => !string.IsNullOrEmpty(line))
        .ToGrid();
    var antennaGroups = map
        .Where(t => t.value != '.')
        .GroupBy(t => t.value)
        .Select(grp => grp.Select(t => t.coordinate).ToArray())
        .ToArray();
    var antennaPairs = antennaGroups
        .SelectMany(antennaGroup => antennaGroup.ToPairs())
        .ToArray();

    var antiNodes = new HashSet<Point>();
    
    foreach (var (a, b) in antennaPairs)
    {
        var line = new Line(a, b);

        antiNodes.Add(a.Add(line.BAVector));
        antiNodes.Add(b.Add(line.ABVector));
    }

    Console.WriteLine($"Part 1: {antiNodes.Where(map.IsValid).Count()}");
    
    antiNodes.Clear();
    
    foreach (var (a, b) in antennaPairs)
    {
        var line = new Line(a, b);

        foreach (var (end, vector) in new[] { (a, line.BAVector), (b, line.ABVector) })
        {
            var point = end;

            while (true)
            {
                point = point.Add(vector);

                if (map.IsValid(point))
                {
                    antiNodes.Add(point);
                }
                else
                {
                    break;
                }
            }
        }
    }

    antiNodes.UnionWith(antennaGroups.SelectMany(antennaGroup => antennaGroup));
    
    Console.WriteLine($"Part 2: {antiNodes.Count}\n");
}
