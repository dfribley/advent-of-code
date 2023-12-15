using AoC.Shared.Strings;
using System.Numerics;

Console.WriteLine("AOC - Day 18\n");

var adjacentPositions = new[]
{
    new Vector3(-1, 0, 0),
    new Vector3(1, 0, 0),
    new Vector3(0, -1, 0),
    new Vector3(0, 1, 0),
    new Vector3(0, 0, -1),
    new Vector3(0, 0, 1)
};

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    var cubes = File.ReadAllLines(inputFile)
        .Where(line => !string.IsNullOrEmpty(line))
        .Select(line =>
        {
            var parts = line.Split(',');

            return new Vector3(parts[0].ToInt32(), parts[1].ToInt32(), parts[2].ToInt32());
        })
        .ToHashSet();

    Console.WriteLine($"[{inputFile}]\n");

    var part1 = cubes
        .Select(c => adjacentPositions.Where(ap => !cubes.Contains(c + ap)).Count())
        .Sum();
    Console.WriteLine($"Part 1: {part1}");

    var first = cubes.First();
    var min = new Vector3(first.X, first.Y, first.Z);
    var max = new Vector3(first.X, first.Y, first.Z);
    var outCubes = new HashSet<Vector3>();
    var airPocketCubes = new HashSet<Vector3>();

    cubes.ToList().ForEach(c =>
    {
        min.X = Math.Min(min.X, c.X);
        min.Y = Math.Min(min.Y, c.Y);
        min.Z = Math.Min(min.Z, c.Z);
        max.X = Math.Max(max.X, c.X);
        max.Y = Math.Max(max.Y, c.Y);
        max.Z = Math.Max(max.Z, c.Z);
    });

    bool canReachOutside(Vector3 position)
    {
        var pathCubes = new List<Vector3>();
        var pathQueue = new Queue<Vector3>();
        var outsidePath = false;

        pathQueue.Enqueue(position);

        while (pathQueue.Any())
        {
            var pathCube = pathQueue.Dequeue();

            if (pathCubes.Contains(pathCube) || cubes.Contains(pathCube) || airPocketCubes.Contains(pathCube))
            {
                continue;
            }

            pathCubes.Add(pathCube);

            if (outCubes.Contains(pathCube) ||
                pathCube.X < min.X || pathCube.Y < min.Y || pathCube.Z < min.Z ||
                pathCube.X > max.X || pathCube.Y > max.Y || pathCube.Z > max.Z)
            {
                outsidePath = true;
                pathCubes.ForEach(c => outCubes.Add(c));
                break;
            }

            foreach (var ap in adjacentPositions)
            {
                pathQueue.Enqueue(pathCube + ap);
            }
        }

        if (!outsidePath)
        {
            pathCubes.ForEach(c => airPocketCubes.Add(c));
        }

        return outsidePath;
    };

    var part2 = cubes
        .Select(c => adjacentPositions.Where(ap => canReachOutside(c + ap)).Count())
        .Sum();
    Console.WriteLine($"Part 2: {part2}\n");
}