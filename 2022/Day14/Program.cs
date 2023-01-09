using AoC.Shared.Strings;
using Shared.Lines;
using System.Drawing;
using System.Numerics;

Console.WriteLine("AOC - Day 13\n");

static int dropTheSand(HashSet<Vector2> rocks, Vector2 start, Func<Vector2, bool> endState)
{
    var sandAtRest = new HashSet<Vector2>();
    var floor = rocks.OrderByDescending(p => p.Y).First().Y + 2;

    while (true)
    {
        var sand = start;

        while (true)
        {
            var isAtRest = true;

            foreach (var dir in new[] { new Vector2(0, 1), new Vector2(-1, 1), new Vector2(1, 1) })
            {
                var newSand = sand + dir;

                if (newSand.Y == floor || rocks.Contains(newSand) || sandAtRest.Contains(newSand))
                {
                    continue;
                }

                if (endState(newSand))
                {
                    return sandAtRest.Count;
                }

                sand = newSand;
                isAtRest = false;
                break;
            }

            if (isAtRest)
            {
                sandAtRest.Add(sand);

                if (endState(sand))
                {
                    return sandAtRest.Count;
                }

                break;
            }
        }
    }
};

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    Console.WriteLine($"[{inputFile}]\n");

    var rocks = new HashSet<Vector2>();

    File.ReadAllLines(inputFile)
        .Where(line => !string.IsNullOrEmpty(line))
        .ToList()
        .ForEach(l =>
        {
            var points = l.Split(" -> ");

            for (var i = 0; i < points.Length - 1; i++)
            {
                var aParts = points[i].Split(',');
                var bParts = points[i + 1].Split(',');

                var line = new Line()
                {
                    PointA = new Point(aParts[0].ToInt32(), aParts[1].ToInt32()),
                    PointB = new Point(bParts[0].ToInt32(), bParts[1].ToInt32())
                };

                foreach (var rockPoint in line.GetPoints().Select(p => new Vector2(p.X, p.Y)))
                {
                    rocks.Add(rockPoint);
                }
            }
        });

    var sandStart = new Vector2(500, 0);

    var abyss = rocks.OrderByDescending(p => p.Y).First().Y + 1;
    var part1 = dropTheSand(rocks, sandStart, p => p.Y == abyss);
    Console.WriteLine($"Part 1: {part1}");

    var part2 = dropTheSand(rocks, sandStart, p => p == sandStart);
    Console.WriteLine($"Part 2: {part2}\n");
}
