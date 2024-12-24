// Advent of Code challenge: https://adventofcode.com/2024/day/14

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
    
    var part1Robots = ActivateRobots(robots, rows, cols, 100);
    var midRow = rows / 2;
    var midCol = cols / 2;
    var quad1 = 0;
    var quad2 = 0;
    var quad3 = 0;
    var quad4 = 0;
    
    foreach (var robot in part1Robots)
    {
        if (robot.pos.X < midCol && robot.pos.Y < midRow)
        {
            quad1++;
        }
        else if (robot.pos.X > midCol && robot.pos.Y < midRow)
        {
            quad2++;
        }
        else if (robot.pos.X < midCol && robot.pos.Y > midRow)
        {
            quad3++;
        }
        else if (robot.pos.X > midCol && robot.pos.Y > midRow)
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
    // This will not be the same for you.
    var part2 = 6577;
    Console.WriteLine($"Part 2: {part2}\n");
    
    var part2Robots = ActivateRobots(robots, rows, cols, part2);
    var pw = new PixelWriter(cols);
    
    for (var y = 0; y < rows; y++)
    {
        for (var x = 0; x < cols; x++)
        {
            var count = part2Robots.Count(r => r.pos == new Point(x, y));
            pw.Write(count > 0 ? $"{count}"[0] : '.');
        }
    }
}

return;

List<(Point pos, Point velocity)> ActivateRobots(List<(Point pos, Point velocity)> robots, int rows, int cols, int secs)
{
    while (secs-- > 0)
    {
        var newRobots = new List<(Point pos, Point velocity)>();

        foreach (var robot in robots)
        {
            var next = robot.pos.Add(robot.velocity);

            if (next.X >= cols)
            {
                next = next with { X = next.X % cols };
            }
            else if (next.X < 0)
            {
                next = next with { X = cols + next.X % cols };
            }

            if (next.Y >= rows)
            {
                next = next with { Y = next.Y % rows };
            }
            else if (next.Y < 0)
            {
                next = next with { Y = rows + next.Y % rows };
            }

            newRobots.Add(robot with { pos = next });
        }

        robots = newRobots;
    }

    return robots;
}
