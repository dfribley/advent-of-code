using AoC.Shared.Strings;
using System.Numerics;

Console.WriteLine("AOC - Day 9\n\n");

static int simulateMotions(IList<(string, int)> moves, int knotCount)
{
    var knots = new List<Vector2>();
    var tailPositions = new HashSet<Vector2>();

    while(knotCount-- > 0)
    {
        knots.Add(new Vector2(0, 0));
    }

    foreach (var (direction, amount) in moves)
    {
        var i = amount;
        while(i-- > 0)
        {
            for (var k = 0; k < knots.Count; k++)
            {
                Vector2 change;

                if (k > 0)
                {
                    var deltaX = knots[k - 1].X - knots[k].X;
                    var deltaY = knots[k - 1].Y - knots[k].Y;

                    change = new Vector2(0, 0);

                    if (Math.Abs(deltaX) > 1 || Math.Abs(deltaY) > 1)
                    {
                        change = new Vector2(
                            Math.Abs(deltaX) > 0 ? deltaX / Math.Abs(deltaX) : 0,
                            Math.Abs(deltaY) > 0 ? deltaY / Math.Abs(deltaY) : 0
                        );
                    }
                }
                else
                {
                    change = direction switch
                    {
                        "U" => new Vector2(0, -1),
                        "D" => new Vector2(0, 1),
                        "L" => new Vector2(-1, 0),
                        "R" => new Vector2(1, 0),
                        _ => throw new NotImplementedException()
                    };
                }

                knots[k] += change;
            }

            var tail = knots.Last();
            if (!tailPositions.Contains(tail))
            {
                tailPositions.Add(tail);
            }
        }
    }

    return tailPositions.Count;
};

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    Console.WriteLine($"[{inputFile}]\n");

    var moves = File.ReadAllLines(inputFile)
        .Select(line =>
        {
            var parts = line.Split(" ");
            var direction = parts[0];
            var amount = parts[1].ToInt32();

            return (direction, amount);
        })
        .ToList();

    var part1 = simulateMotions(moves, 2);
    Console.WriteLine($"Part 1: {part1}");

    var part2 = simulateMotions(moves, 10);
    Console.WriteLine($"Part 2: {part2}\n");
}