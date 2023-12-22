using System.Numerics;
using AoC.Shared.Strings;

Console.WriteLine("AOC - Day 22\n\n");

foreach (var inputFile in new[] { "sample.txt" ,"input.txt" })
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    Console.WriteLine($"[{inputFile}]\n");

    var bricks = File.ReadAllLines(inputFile)
        .Where(line => !string.IsNullOrEmpty(line))
        .Select((line, i) =>
        {
            var ends = line.Split("~")
                .Select(side =>
                {
                    var sideParts = side.Split(",");
                    return new Vector3(sideParts[0].ToInt32(), sideParts[1].ToInt32(), sideParts[2].ToInt32());
                })
                .ToList();

            var end1 = ends.First();
            var end2 = ends.Last();

            var points = new List<Vector3>();

            if (end1.X != end2.X)
            {
                for (
                    int x = Math.Min((int)end1.X, (int)end2.X);
                    x <= Math.Max((int)end1.X, (int)end2.X);
                    x++)
                {
                    points.Add(new Vector3(x, end1.Y, end1.Z));
                }
            }
            else if (end1.Y != end2.Y)
            {
                for (
                    int y = Math.Min((int)end1.Y, (int)end2.Y);
                    y <= Math.Max((int)end1.Y, (int)end2.Y);
                    y++)
                {
                    points.Add(new Vector3(end1.X, y, end1.Z));
                }
            }
            else
            {
                for (
                    int z = Math.Min((int)end1.Z, (int)end2.Z);
                    z <= Math.Max((int)end1.Z, (int)end2.Z);
                    z++)
                {
                    points.Add(new Vector3(end1.X, end1.Y, z));
                }
            }

            return (id: i, minZ: Math.Min(end1.Z, end2.Z), points);
        })
        .ToList();

    // Settle bricks

    var settledPoints = new Dictionary<Vector3, int>();
    var drop = new Vector3(0, 0, -1);

    bricks.OrderBy(b => b.minZ)
        .ToList()
        .ForEach((brick) =>
        {
            while (!brick.points.Any(p => p.Z == 1) && brick.points.All(v => !settledPoints.ContainsKey(v + drop)))
            {
                brick.points = brick.points.Select(p => p + drop).ToList();
            }

            foreach (var p in brick.points)
            {
                settledPoints.Add(p, brick.id);
            }
        });

    // Create contact map

    var map = new Dictionary<int, IEnumerable<int>>();
    var up = new Vector3(0, 0, 1);

    foreach (var id in bricks.Select(b => b.id))
    {
        var bricksAbove = new List<int>();

        foreach (var p in settledPoints
            .Where(kvp => kvp.Value == id)
            .Select(kvp => kvp.Key))
        {
            if (settledPoints.TryGetValue(p + up, out var onTop) &&
                !bricksAbove.Contains(onTop) &&
                onTop != id)
            {
                bricksAbove.Add(onTop);
            }
        }

        map.Add(id, bricksAbove);
    }

    // Find bricks that can be disintegrated without having other bricks fall

    var part1 = bricks
        .Where(brick => map[brick.id]
            .All(top => map
                .Any(bottom => bottom.Key != brick.id && bottom.Value.Contains(top))))
        .Count();

    Console.WriteLine($"Part 1: {part1}");

    // Determine which bricks would fall if a specific brick as disintegrated

    int wouldFall(List<int> fell)
    {
        while (true)
        {
            var falling = fell
                .SelectMany(fBrick => map[fBrick]
                    .Where(top => !fell.Contains(top) && !map.Any(bottom => !fell.Contains(bottom.Key) && bottom.Value.Contains(top)))
                    .ToArray())
                .Distinct()
                .ToArray();

            if (falling.Length == 0)
            {
                break;
            }

            fell.AddRange(falling);
        }

        return fell.Count;
    }

    var part2 = bricks
        .Select(b => wouldFall(new List<int>() { b.id }) - 1)
        .Sum();

    Console.WriteLine($"Part 2: {part2}\n");
}
