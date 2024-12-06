// Advent of Code challenge: https://adventofcode.com/2024/day/6

using System.Drawing;
using AoC.Shared.Grid;
using AoC.Shared.Points;

Console.WriteLine("AoC - Day 6\n\n");

const string startChars = "^>v<";

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

    var start = map.First(startChars.Contains);
    var startDir = GridDirections.North.RotateRight(startChars.IndexOf(map[start]) * 90);
    
    var positions = new HashSet<Point>();
    var position = start;
    var dir = startDir;
    
    while(true)
    {
        positions.Add(position);
        
        var next = position.Add(dir);
        
        if (!map.IsValid(next))
        {
            break;
        }

        if (map[next] == '#')
        {
            dir = dir.RotateRight(90);
        }
        else
        {
            position = next;
        }
    }
    
    Console.WriteLine($"Part 1: {positions.Count}");

    var part2 = map
        .Where(chr => chr == '.')
        .Count(block =>
        {
            position = start;
            dir = startDir;
            var knownStates = new HashSet<(Point, Point)>();

            while(true)
            {
                var state = (position, dir);
                if (!knownStates.Add(state))
                {
                    return true;
                }

                var next = position.Add(dir);

                if (!map.IsValid(next))
                {
                    return false;
                }

                if (map[next] == '#' || next == block)
                {
                    dir = dir.RotateRight(90);
                }
                else
                {
                    position = next;
                }
            }
        });
    
    Console.WriteLine($"Part 2: {part2}\n");
}
