// Advent of Code challenge: https://adventofcode.com/2024/day/12

using System.Drawing;
using AoC.Shared.Collections;
using AoC.Shared.Grid;
using AoC.Shared.Points;

Console.WriteLine("AoC - Day 12\n\n");

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    Console.WriteLine($"[{inputFile}]\n");

    var garden = File.ReadAllLines(inputFile)
        .Where(line => !string.IsNullOrEmpty(line))
        .ToGrid();

    var plots = new List<(int count, Dictionary<Point, List<Point>> edges)>();
    var processed = new HashSet<Point>();

    foreach (var (xy, letter) in garden)
    {
        if (processed.Contains(xy))
        {
            continue;
        }

        var queue = new Queue<Point> { xy };
        var points = new HashSet<Point>();
        var edges = new Dictionary<Point, List<Point>>();

        while (queue.TryDequeue(out var current))
        {
            if (!points.Add(current))
            {
                continue;
            }

            foreach (var dir in GridDirections.SideNeighbors)
            {
                var next = current.Add(dir);

                if (garden.IsValid(next) && garden[next] == letter)
                {
                    queue.Add(next);
                }
                else
                {
                    if (!edges.ContainsKey(dir))
                    {
                        edges[dir] = [];
                    }

                    edges[dir].Add(current);
                }
            }
        }
        
        plots.Add((points.Count, edges));
        processed.UnionWith(points);
    }
    
    var part1 = plots
        .Select(p => p.count * p.edges.Values.Select(lst => lst.Count).Sum())
        .Sum();
    
    Console.WriteLine($"Part 1: {part1}");

    var part2 = plots
        .Select(plt =>
        {
            var sides = 0;
            
            foreach (var kvp in plt.edges)
            {
                sides++;
                if (kvp.Key.Y == 0)
                {
                    var ary = kvp.Value.OrderBy(p => p.X).ThenBy(p => p.Y).ToArray();

                    for (var i = 1; i < ary.Length; i++)
                    {
                        if (ary[i].X != ary[i - 1].X || ary[i].Y != ary[i - 1].Y + 1)
                        {
                            sides++;
                        }
                    }
                }
                else
                {
                    var ary = kvp.Value.OrderBy(p => p.Y).ThenBy(p => p.X).ToArray();

                    for (var i = 1; i < ary.Length; i++)
                    {
                        if (ary[i].Y != ary[i - 1].Y || ary[i].X != ary[i - 1].X + 1)
                        {
                            sides++;
                        }
                    }
                }
            }
            
            return plt.count * sides;
        })
        .Sum();
    
    Console.WriteLine($"Part 2: {part2}\n");
}
