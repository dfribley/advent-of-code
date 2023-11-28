using System.Diagnostics;
using System.Numerics;
using AoC.Shared.Distance;
using AoC.Shared.Strings;

Console.WriteLine("AOC - Day 15\n\n");

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    Console.WriteLine($"[{inputFile}]\n");

    var input = File.ReadAllLines(inputFile)
        .Where(line => !string.IsNullOrEmpty(line))
        .ToList();

    var maxRow = input.Count;
    var maxCol = input[0].Length;

    var riskLevels = input
        .SelectMany((row, r) => row.Select((level, c) => (position: new Vector2(r, c), risk: level.ToInt32())))
        .ToDictionary(t => t.position, t => t.risk);

    var neighbors = new[] { new Vector2(-1, 0), new Vector2(0, 1), new Vector2(0, -1), new Vector2(1, 0) };
    var start = new Vector2(0, 0);

    int solve(int tiles)
    {
        var knownLevels = new Dictionary<Vector2, int>();
        var queue = new PriorityQueue<(Vector2 position, int riskToPosition), int>();
        
        queue.Enqueue((start, 0), 0);

        while (queue.Count > 0)
        {
            var (position, riskToPosition) = queue.Dequeue();

            if (position.X < 0 || position.X >= maxRow * tiles || position.Y < 0 || position.Y >= maxCol * tiles)
            {
                continue;
            }

            var additionalRisk = riskLevels[new Vector2(position.X % maxRow, position.Y % maxCol)] + (int)(position.X / maxRow) + (int)(position.Y / maxCol);
            additionalRisk = (additionalRisk - 1) % 9 + 1;

            riskToPosition += additionalRisk;

            if (!knownLevels.ContainsKey(position) || riskToPosition < knownLevels[position])
            {
                knownLevels[position] = riskToPosition;
            }
            else
            {
                continue;
            }

            foreach (var change in neighbors)
            {
                var neighbor = position + change;

                queue.Enqueue((position + change, riskToPosition), TaxiCab.GetDistance(start, neighbor));
            }
        }

        return knownLevels[new Vector2(maxRow * tiles - 1, maxCol * tiles - 1)] - riskLevels[new Vector2(0,0)];
    }

    var sw = new Stopwatch();
    sw.Start();
    var part1 = solve(1);
    sw.Stop();

    Console.WriteLine($"Part 1: ({string.Format("{0:00}:{1:00}.{2:00}", sw.Elapsed.Minutes, sw.Elapsed.Seconds, sw.Elapsed.Milliseconds / 10)}) {part1}");

    sw.Restart();
    var part2 = solve(5);
    sw.Stop();

    Console.WriteLine($"Part 2: ({string.Format("{0:00}:{1:00}.{2:00}", sw.Elapsed.Minutes, sw.Elapsed.Seconds, sw.Elapsed.Milliseconds / 10)}) {part2}\n");
}