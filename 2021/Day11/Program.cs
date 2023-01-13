using System.Numerics;

Console.WriteLine("AOC - Day 11\n\n");

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    Console.WriteLine($"[{inputFile}]\n");

    var input = File.ReadAllLines(inputFile)
        .Where(line => !string.IsNullOrEmpty(line))
        .ToList();
    var rowMax = input.Count;
    var colMax = input[0].Length;

    var octopuses = input
        .SelectMany((row, r) => row.Trim().Select((col, c) => (position: new Vector2(r, c), energy: Convert.ToInt32(col.ToString()))))
        .ToDictionary(t => t.position, t => t.energy);

    (int flashes, int steps) runSimulation(IDictionary<Vector2, int> octopuses, Func<(int steps, bool allOn), bool> endState)
    {
        var flashes = 0;

        void flashOctopus(Vector2 position)
        {
            octopuses[position] = -1;
            flashes++;

            foreach(var delta in new [] { new Vector2(-1, -1), new Vector2(-1, 0), new Vector2(-1, 1), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0), new Vector2(1, -1), new Vector2(0, -1) })
            {
                var neighbor = position + delta;

                if ((neighbor.X >= 0 && neighbor.X < rowMax) && (neighbor.Y >= 0 && neighbor.Y < colMax) && octopuses[neighbor] != -1)
                {
                    octopuses[neighbor]++;
                    if (octopuses[neighbor] > 9)
                    {
                        flashOctopus(neighbor);
                    }
                }
            }
        }

        var step = 0;
        var maxSteps = 1000;

        while (++step < maxSteps)
        {
            foreach (var octopus in octopuses.Keys)
            {
                octopuses[octopus]++;
            }

            foreach (var octopus in octopuses.Where(kvp => kvp.Value > 9).ToList())
            {
                if (octopuses[octopus.Key] != -1)
                {
                    flashOctopus(octopus.Key);
                }
            }

            foreach (var octopus in octopuses.Where(kvp => kvp.Value == -1).ToList())
            {
                octopuses[octopus.Key] = 0;
            }

            if (endState((step, octopuses.All(kvp => kvp.Value == 0))))
            {
                return (flashes, step);
            }
        }

        throw new Exception("Max rounds reached!");
    }

    var octopusesCopy = new Dictionary<Vector2, int>(octopuses);
    var part1 = runSimulation(octopusesCopy, t => t.steps == 100).flashes;
    Console.WriteLine($"Part 1: {part1}");

    var part2 = runSimulation(octopuses, t => t.allOn).steps;
    Console.WriteLine($"Part 2: {part2}\n");
}