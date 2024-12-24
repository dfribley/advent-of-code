// Advent of Code challenge: https://adventofcode.com/2024/day/18

using System.Drawing;
using AoC.Shared.Collections;
using AoC.Shared.Grid;
using AoC.Shared.Points;
using AoC.Shared.Strings;

Console.WriteLine("AoC - Day 18\n\n");

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    Console.WriteLine($"[{inputFile}]\n");

    var bytes = File.ReadAllLines(inputFile)
        .Where(line => !string.IsNullOrEmpty(line))
        .Select(line =>
        {
            var lineParts = line.Split(",");

            return new Point(lineParts[0].ToInt32(), lineParts[1].ToInt32());
        })
        .ToArray();

    var setupBytes = inputFile.StartsWith("sample") ? 12 : 1024;
    var firstBytes = bytes.Take(setupBytes).ToArray();
    var maxX = 0;
    var maxY = 0;
    
    foreach (var bytePoint in firstBytes)
    {
        maxX = Math.Max(maxX, bytePoint.X);
        maxY = Math.Max(maxY, bytePoint.Y);
    }
    
    var part1 = FindMinimumStepsToExit(firstBytes.ToHashSet(), maxX, maxY);
    Console.WriteLine($"Part 1: {part1}");

    // Binary Search for part 2
    var low = setupBytes + 1;
    var high = bytes.Length;

    while (true)
    {
        var mid = (low + high) / 2;

        if (low == mid)
        {
            var blocker = FindMinimumStepsToExit(bytes.Take(low).ToHashSet(), maxX, maxY) == -1 ?
                bytes[low - 1] :
                bytes[high - 1];
            
            Console.WriteLine($"Part 2: {blocker.X},{blocker.Y}\n");
            break;
        }
        
        if (FindMinimumStepsToExit(bytes.Take(mid).ToHashSet(), maxX, maxY) == -1)
        {
            high = mid;
        }
        else
        {
            low = mid;
        }
    }
}

return;

int FindMinimumStepsToExit(HashSet<Point> obstacles, int maxX, int maxY)
{
    var start = new Point(0, 0);
    var end = new Point(maxX, maxY);
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
            return path.Count - 1;
        }

        foreach (var dir in GridDirections.SideNeighbors)
        {
            var next = current.Add(dir);

            if (path.Contains(next) || obstacles.Contains(next) || 
                next.X < 0 || next.Y < 0 || next.X > maxX || next.Y > maxY)
            {
                continue;
            }

            queue.Enqueue((next, path.Append(next).ToList()));
        }
    }

    return -1;
}
