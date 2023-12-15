using AoC.Shared.Strings;
using System.Numerics;

Console.WriteLine("AOC - Day 9\n\n");

var neighbors = new[] { new Vector2(0, -1), new Vector2(-1, 0), new Vector2(1, 0), new Vector2(0, 1) };

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

    var spots = input.SelectMany((row, r) => row.Select((spot, c) => (position: new Vector2(r, c), height: spot.ToInt32())))
        .ToDictionary(t => t.position, t => t.height);

    var lowSpots = spots
        .Where(kvp => 
            neighbors.All(n => 
            {
                var adjacentPosition = kvp.Key + n;

                if (adjacentPosition.X >= 0 && adjacentPosition.X < input.Count &&
                    adjacentPosition.Y >= 0 && adjacentPosition.Y < input[0].Length)
                {
                    return spots[adjacentPosition] > kvp.Value;
                }

                return true;
            })
        )
        .ToList();

    var part1 = lowSpots
        .Select(kvp => kvp.Value + 1)
        .Sum();

    Console.WriteLine($"Part 1: {part1}");

    IEnumerable<Vector2> getBasinPoints(Vector2 position)
    {
        var queue = new Queue<Vector2>();
        var basin = new HashSet<Vector2>();
        queue.Enqueue(position);

        while (queue.Any())
        {
            var me = queue.Dequeue();
            basin.Add(me);

            foreach(var ap in neighbors)
            {
                var neighbor = me + ap;

                if (neighbor.X >= 0 && neighbor.X < input.Count &&
                    neighbor.Y >= 0 && neighbor.Y < input[0].Length &&
                    !basin.Contains(neighbor) && 
                    spots[neighbor] != 9 && spots[neighbor] > spots[me])
                {
                    queue.Enqueue(neighbor);
                }
            }
        }

        return basin;
    }

    var part2 = 1;
    lowSpots
        .Select(kvp => getBasinPoints(kvp.Key).Count())
        .OrderDescending()
        .Take(3)
        .ToList()
        .ForEach(c => part2 *= c);

    Console.WriteLine($"Part 2: {part2}\n");
}