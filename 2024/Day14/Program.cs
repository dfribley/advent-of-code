// Advent of Code challenge: https://adventofcode.com/2024/day/14

using System.Diagnostics;
using System.Drawing;
using AoC.Shared.PixelWriter;
using AoC.Shared.Points;
using AoC.Shared.Strings;

Console.WriteLine("AoC - Day 14\n\n");

foreach (var inputFile in new[] {"sample.txt", "input.txt" })
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    Console.WriteLine($"[{inputFile}]\n");

    var robots = File.ReadAllLines(inputFile)
        .Where(line => !string.IsNullOrEmpty(line))
        .Select(ln =>
        {
            var parts = ln.Split(" ");
            var robotParts = parts[0].Split("=");
            var robotpos = robotParts[1].Split(",");

            var velocityParts = parts[1].Split("=");
            var velocity = velocityParts[1].Split(",");

            return (pos: new Point(robotpos[0].ToInt32(), robotpos[1].ToInt32()),
                velocity: new Point(velocity[0].ToInt32(), velocity[1].ToInt32()));
        })
        .ToList();

    var rows = 7;
    var cols = 11;
    
    if (inputFile.StartsWith("input"))
    {
        rows = 103;
        cols = 101;
    }

    var sw = new Stopwatch();
    sw.Start();
    
    var part1Robots = ActivateRobots(robots, rows, cols, 100);
    
    var midRow = rows / 2;
    var midCol = cols / 2;
    var quad1 = 0;
    var quad2 = 0;
    var quad3 = 0;
    var quad4 = 0;
    
    foreach (var robot in part1Robots)
    {
        if (robot.X < midCol && robot.Y < midRow)
        {
            quad1++;
        }
        else if (robot.X > midCol && robot.Y < midRow)
        {
            quad2++;
        }
        else if (robot.X < midCol && robot.Y > midRow)
        {
            quad3++;
        }
        else if (robot.X > midCol && robot.Y > midRow)
        {
            quad4++;
        }
    }

    Console.WriteLine($"Part 1: {quad1 * quad2 * quad3 * quad4}");
    
    if (inputFile.StartsWith("sample"))
    {
        Console.WriteLine();
        continue;
    }

    // For part2, I looked for states where the number of robots in a given row was above 30,
    // printed the grid and then looped until I saw a Christmas tree!
    // Note: 6577 is the number of seconds it took to see the Christmas tree for my input.
    // --
    // Then I saw a comment that the tree state was also where all the robot positions were unique.
    var part2 = 0;

    while (true)
    {
        var p2Robots = ActivateRobots(robots, rows, cols, part2);

        if (!p2Robots.GroupBy(r => r).Any(g => g.Count() > 1))
        {
            break;
        }

        part2++;
    }
    
    Console.WriteLine($"Part 2: {part2}\n");
    
    var part2Robots = ActivateRobots(robots, rows, cols, part2);
    var pw = new PixelWriter(cols);
    
    for (var y = 0; y < rows; y++)
    {
        for (var x = 0; x < cols; x++)
        {
            var count = part2Robots.Count(r => r == new Point(x, y));
            pw.Write(count > 0 ? $"{count}"[0] : '.');
        }
    }
    
    sw.Stop();
    
    Console.WriteLine($"Time: {sw.ElapsedMilliseconds}");
}

return;

Point[] ActivateRobots(List<(Point pos, Point velocity)> robots, int rows, int cols, int secs)
{
    return robots
        .Select(r =>
        {
            var pos = r.pos.Add(r.velocity.Multiply(secs));
            
            if (pos.X >= cols)
            {
                pos = pos with { X = pos.X % cols };
            }
            else if (pos.X < 0)
            {
                pos = pos with { X = cols + (pos.X + 1) % cols - 1 };
            }
            
            if (pos.Y >= rows)
            {
                pos = pos with { Y = pos.Y % rows };
            }
            else if (pos.Y < 0)
            {
                pos = pos with { Y = rows + (pos.Y + 1) % rows - 1 };
            }

            return pos;
        })
        .ToArray();
}
