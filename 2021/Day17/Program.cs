using System.Numerics;
using System.Text.RegularExpressions;
using AoC.Shared.Strings;

Console.WriteLine("AOC - Day 17\n");

var inputRegEx = new Regex(@"target area: x=(-?\d+)..(-?\d+), y=(-?\d+)..(-?\d+)");

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    Console.WriteLine($"[{inputFile}]\n");

    var match = inputRegEx.Match(File.ReadAllText(inputFile).Trim());

    var min = new Vector2(match.Groups[1].Value.ToInt32(), match.Groups[4].Value.ToInt32());
    var max = new Vector2(match.Groups[2].Value.ToInt32(), match.Groups[3].Value.ToInt32());

    (bool hits, float highestY) hitsTarget(Vector2 velocity)
    {
        var position = new Vector2(0, 0);
        var hightestY = 0f;

        while (true)
        {
            position += velocity;

            if (position.Y > hightestY)
            {
                hightestY = position.Y;
            }

            if (position.X > max.X || position.Y < max.Y)
            {
                return (false, hightestY);
            }

            if (position.X >= min.X && position.Y <= min.Y)
            {
                return (true, hightestY);
            }

            if (velocity.X != 0)
            {
                velocity.X += -velocity.X / Math.Abs(velocity.X);
            }

            velocity.Y -= 1;
        }
    }

    var hits = 0;
    var highestY = 0f;

    for (var x = 1; x <= max.X; x++)
    {
        for (var y = max.Y; y <= Math.Abs(max.Y); y++)
        {
            var result = hitsTarget(new Vector2(x, y));

            if (result.hits)
            {
                hits++;

                if (result.highestY > highestY)
                {
                    highestY = result.highestY;
                }
            }
        }
    }

    Console.WriteLine($"Part 1: {highestY}");
    Console.WriteLine($"Part 2: {hits}\n");
}
