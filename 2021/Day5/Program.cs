using System.Drawing;
using AoC.Shared.Lines;

Console.WriteLine("AOC - Day 5\n\n");

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    Console.WriteLine($"[{inputFile}]\n");

    var lines = File.ReadAllLines(inputFile)
        .Select(line =>
        {
            var points = line.Split(" -> ");
            var pointAParts = points[0].Split(",");
            var pointBParts = points[1].Split(",");

            return new Line
            {
                PointA = new Point(Convert.ToInt32(pointAParts[0]), Convert.ToInt32(pointAParts[1])),
                PointB = new Point(Convert.ToInt32(pointBParts[0]), Convert.ToInt32(pointBParts[1])),
            };
        });

    var part1 = lines
        .Where(line => line.IsHorizontal() || line.IsVertical())
        .SelectMany(line => line.GetPoints())
        .GroupBy(point => point)
        .Where(group => group.Count() > 1)
        .Count();

    Console.WriteLine($"Part 1: {part1}");

    var part2 = lines
        .Where(line => line.IsHorizontal() || line.IsVertical() || line.IsDiaganol())
        .SelectMany(line => line.GetPoints())
        .GroupBy(point => point)
        .Where(group => group.Count() > 1)
        .Count();

    Console.WriteLine($"Part 2: {part2}\n");
}