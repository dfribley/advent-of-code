using System.Numerics;

Console.WriteLine("AOC - Day 12\n");

static int findFastestPath(IDictionary<Vector2, int> elevations, IEnumerable<Vector2> startingPoints, Vector2 endPoint)
{
    var queue = new Queue<(Vector2, int)>();

    foreach (var point in startingPoints)
    {
        queue.Enqueue((point, 0));
    }

    var knownPoints = new HashSet<Vector2>();

    while (queue.Any())
    {
        var (point, steps) = queue.Dequeue();

        if (knownPoints.Contains(point))
        {
            continue;
        }

        knownPoints.Add(point);

        if (point == endPoint)
        {
            return steps;
        }

        foreach (var direction in new[] { new Vector2(-1, 0), new Vector2(0, 1), new Vector2(1, 0), new Vector2(0, -1) })
        {
            var next = point + direction;

            if (elevations.ContainsKey(next) && (elevations[next] <= elevations[point] + 1))
            {
                queue.Enqueue((next, steps + 1));
            }
        }
    }

    throw new Exception("Unable to reach the end!");
};

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    Console.WriteLine($"[{inputFile}]\n");

    var start = default(Vector2);
    var end = default(Vector2);

    var elevations = File.ReadAllLines(inputFile)
        .Where(line => !string.IsNullOrEmpty(line))
        .SelectMany((row, r) =>
        {
            return row.Select((elevation, c) =>
            {
                var square = new Vector2(r, c);
                if (elevation == 'S')
                {
                    start = square;
                    return (square, elevation: (int)'a');
                }

                if (elevation == 'E')
                {
                    end = square;
                    return (square, elevation: (int)'z');
                }

                return (square, elevation: (int)elevation);
            });
        })
        .ToDictionary(t => t.square, t => t.elevation);

    var part1 = findFastestPath(elevations, new[] { start }, end);
    Console.WriteLine($"Part 1: {part1}");

    var part2 = findFastestPath(elevations, elevations.Where(kvp => kvp.Value == (int)'a').Select(kvp => kvp.Key), end);
    Console.WriteLine($"Part 2: {part2}\n");
}