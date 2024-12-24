// Advent of Code challenge: https://adventofcode.com/2024/day/16

using System.Drawing;
using AoC.Shared.Grid;
using AoC.Shared.Points;

Console.WriteLine("AoC - Day 16\n\n");

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
    
    var start = map.First(chr => chr == 'S');
    var knownScores = new Dictionary<(Point, Point), int>();
    var bestPathSpots = new HashSet<Point>();
    var bestScore = int.MaxValue;
    var queue = new PriorityQueue<(Point, Point, HashSet<Point>), int>();
    
    queue.Enqueue((start, GridDirections.East, [start]), 0);

    while (queue.TryDequeue(out var element, out var score))
    {
        var (xy, dir, path) = element;
        var stateKey = (xy, dir);
        
        if (score > bestScore || knownScores.TryGetValue(stateKey, out var value) && score > value)
        {
            continue;
        }
        
        knownScores[stateKey] = score;

        if (map[xy] == 'E')
        {
            bestScore = score;
            bestPathSpots.UnionWith(path);
            continue;
        }
        
        foreach (var ndir in GridDirections.SideNeighbors)
        {
            var next = xy.Add(ndir);
            
            if (ndir != dir.RotateRight(180) && map[next] != '#')
            {
                if (path.Contains(next))
                {
                    continue;
                }
                
                var pathknown = new HashSet<Point>(path) { next };

                queue.Enqueue((next, ndir, pathknown), score + (ndir == dir ? 1 : 1001));
            }
        }
    }
    
    Console.WriteLine($"Part 1: {bestScore}");
    Console.WriteLine($"Part 2: {bestPathSpots.Count}\n");
}
