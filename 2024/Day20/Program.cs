// Advent of Code challenge: https://adventofcode.com/2024/day/20

using System.Drawing;
using AoC.Shared.Collections;
using AoC.Shared.Distance;
using AoC.Shared.Grid;
using AoC.Shared.Points;

Console.WriteLine("AoC - Day 20\n\n");

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
    var end = map.First(chr => chr == 'E');
    var walls = map.Where(chr => chr == '#').ToHashSet();

    var minPicoseconds = inputFile.StartsWith("sample") ? 50 : 100;

    Console.WriteLine($"Part 1: {GetCheatsThatSaveAtLeast(minPicoseconds, 2)}");
    Console.WriteLine($"Part 2: {GetCheatsThatSaveAtLeast(minPicoseconds, 20)}\n");
    
    continue;

    int GetCheatsThatSaveAtLeast(int picoseconds,int maxCheat)
    {
        var (time, path) = GetPathToEnd(start, end, walls);
        var totalCheats = 0;
    
        for (var i = 0; i < path.Count; i++)
        {
            for (var j = i + 1; j < path.Count; j++)
            {
                var cheatDistance = TaxiCab.GetDistance(path[i], path[j]);

                if (cheatDistance > maxCheat)
                {
                    continue;
                }
                
                var newTime = i + cheatDistance + path.Count - j - 1;
                
                if (time - newTime >= picoseconds)
                {
                    totalCheats++;
                }
            }
        }

        return totalCheats;
    }
}

return;

(int time, List<Point> path) GetPathToEnd(Point start, Point end, HashSet<Point> walls)
{
    var queue = new Queue<(Point, List<Point>)> { (start, [start]) };
    var seen = new HashSet<Point>();

    while (queue.TryDequeue(out var element))
    {
        var (current, path) = element;

        if (!seen.Add(current))
        {
            continue;
        }

        if (current == end)
        {
            return (path.Count - 1, path);
        }

        foreach (var dir in GridDirections.SideNeighbors)
        {
            var next = current.Add(dir);

            if (path.Contains(next) || walls.Contains(next))
            {
                continue;
            }

            queue.Enqueue((next, path.Append(next).ToList()));
        }
    }

    throw new Exception("No path found");
}
