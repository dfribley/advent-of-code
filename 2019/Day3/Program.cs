// Advent of Code challenge: https://adventofcode.com/2019/day/3

using System.Drawing;
using AoC.Shared.Distance;
using AoC.Shared.Lines;
using AoC.Shared.Points;

Console.WriteLine("AoC - Day 3\n\n");

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    Console.WriteLine($"[{inputFile}]\n");

    var input = File.ReadAllLines(inputFile)
        .Where(line => !string.IsNullOrEmpty(line))
        .ToList();

    var line1 = input[0].Split(",").ToList();
    var line2 = input[1].Split(",").ToList();

    var lines1 = ParseLines(line1, new Point(0, 0));
    var lines2 = ParseLines(line2, new Point(0, 0));
    
    var intersections = new Dictionary<Point, int>();
    var intersectionSteps = new List<long>();
    
    foreach (var (l1, s1) in lines1)
    {
        foreach (var (l2, s2) in lines2)
        {
            var points = l1.GetIntersectionPoints(l2);

            if (points.Count != 0)
            {
                var point = points.First();

                if (point != Point.Empty)
                {
                    intersections[point] = TaxiCab.GetDistance(new Point(0, 0), point);
                    intersectionSteps.Add(s1 + s2 + TaxiCab.GetDistance(l1.PointA, point) + TaxiCab.GetDistance(l2.PointA, point));
                }
            }
        }
    }

    Console.WriteLine($"Part 1: {intersections.Values.Min()}");
    Console.WriteLine($"Part 2: {intersectionSteps.Min()}\n");
}

return;

static IList<(Line, int)> ParseLines(IEnumerable<string> lines, Point start)
{
    var result = new List<(Line, int)>();
    var steps = 0;
    
    foreach (var line in lines)
    {
        var dir = line[0];
        var distance = int.Parse(line[1..]);
        
        var end = dir switch
        {
            'R' => start.AddX(distance),
            'L' => start.AddX(-distance),
            'U' => start.AddY(distance),
            'D' => start.AddY(-distance),
            _ => throw new Exception($"Invalid segment: {line}")
        };
        
        result.Add((new Line(start, end), steps));
        start = end;
        steps += distance;
    }
    
    return result;
}
