using System.Drawing;
using AoC.Shared.Distance;
using AoC.Shared.Grid;
using AoC.Shared.Points;
using AoC.Shared.Strings;

Console.WriteLine("AOC - Day 12\n\n");

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    Console.WriteLine($"[{inputFile}]\n");

    var instructions = File.ReadAllLines(inputFile)
        .Where(line => !string.IsNullOrEmpty(line))
        .ToList();

    Console.WriteLine($"Part 1: {Navigate()}");
    Console.WriteLine($"Part 2: {Navigate(false)}\n");
    continue;

    int Navigate(bool part1 = true)
    {
        var waypoint = part1 ? GridDirections.East : new Point(10, 1);
        var pos = new Point(0, 0);
    
        foreach (var instruction in instructions)
        {
            var action = instruction[0];
            var value = instruction[1..].ToInt32();

            if (part1)
            {
                switch (action)
                {
                    case 'N':
                        pos = pos.Add(GridDirections.North.Multiply(value));
                        break;
                    case 'S':
                        pos = pos.Add(GridDirections.South.Multiply(value));
                        break;
                    case 'E':
                        pos = pos.Add(GridDirections.East.Multiply(value));
                        break;
                    case 'W':
                        pos = pos.Add(GridDirections.West.Multiply(value)); 
                        break;
                    case 'L':
                        waypoint = waypoint.RotateLeft(value);
                        break;
                    case 'R':
                        waypoint = waypoint.RotateRight(value);
                        break;
                    case 'F':
                        pos = pos.Add(waypoint.Multiply(value));
                        break;
                } 
            }
            else
            {
                switch (action)
                {
                    case 'N':
                        waypoint = waypoint.Add(GridDirections.North.Multiply(value));
                        break;
                    case 'S':
                        waypoint = waypoint.Add(GridDirections.South.Multiply(value));
                        break;
                    case 'E':
                        waypoint = waypoint.Add(GridDirections.East.Multiply(value));
                        break;
                    case 'W':
                        waypoint = waypoint.Add(GridDirections.West.Multiply(value)); 
                        break;
                    case 'L':
                        waypoint = waypoint.RotateLeft(Point.Empty, value);
                        break;
                    case 'R':
                        waypoint = waypoint.RotateRight(Point.Empty, value);
                        break;
                    case 'F':
                        pos = pos.Add(waypoint.Multiply(value));
                        break;
                }
            }
        }
        
        return TaxiCab.GetDistance(Point.Empty, pos);
    }
}
