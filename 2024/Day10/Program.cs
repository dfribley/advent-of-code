// Advent of Code challenge: https://adventofcode.com/2024/day/10

using System.Drawing;
using AoC.Shared.Grid;
using AoC.Shared.Points;
using AoC.Shared.Strings;

Console.WriteLine("AoC - Day 10\n\n");

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
    
    var queue = new Queue<(Point start, Point current, int height)>(
        map.Where(num => num == '0').Select(p => (p, p, 0))
    );

    var scores = new Dictionary<Point, HashSet<Point>>();
    var distinctPaths = 0;
    
    while (queue.Count > 0)
    {
        var (start, current, height) = queue.Dequeue();

        if (height++ == 9)
        {
            if (!scores.ContainsKey(start))
            {
                scores[start] = [];
            }
            
            scores[start].Add(current);
            distinctPaths++;
            
            continue;
        }

        foreach (var neighbor in GridDirections.SideNeighbors)
        {
            var next = current.Add(neighbor);

            if (map.IsValid(next) && map[next].ToInt32() == height)
            {
                queue.Enqueue((start, next, height));
            }
        }
    }

    Console.WriteLine($"Part 1: {scores.Values.Select(hs => hs.Count).Sum()}");
    Console.WriteLine($"Part 2: {distinctPaths}\n");
}
