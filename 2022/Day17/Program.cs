using System.Collections;
using System.Text;
using AoC.Shared.Collections;
using AoC.Shared.Points;
using P = System.Drawing.Point;

var shapes = new[]
{
    new [] { new P(0, 0), new P(1, 0), new P(2, 0), new P(3, 0) },
    new [] { new P(1, 0), new P(1, 1), new P(1, 2), new P(0, 1), new P(2, 1) },
    new [] { new P(0, 0), new P(1, 0), new P(2, 0), new P(2, 1), new P(2, 2) },
    new [] { new P(0, 0), new P(0, 1), new P(0, 2), new P(0, 3) },
    new [] { new P(0, 0), new P(0, 1), new P(1, 0), new P(1, 1) }
};

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    var jets = File.ReadAllText(inputFile).Trim();

    long getTowerHeight(long numberOfRocks)
    {
        var jetIndex = 0;
        var shapeIndex = 0;
        var height = 0;
        var gameWindow = 50;
        var heightAdjust = 0L;
        var rocksAtRest = new Dictionary<int, HashSet<P>>();
        var states = new Dictionary<string, (long rocks, int height)>();
        var lookForShortcut = true;

        bool containsRock(P point)
        {
            return rocksAtRest.ContainsKey(point.Y) && rocksAtRest[point.Y].Contains(point);
        }

        while (numberOfRocks-- > 0)
        {
            var jet = true;
            var start = new P(2, height + 3);
            var rock = shapes[shapeIndex].Select(p => start.Add(p)).ToList();

            while (true)
            {
                if (jet)
                {
                    var movement = jets[jetIndex] switch
                    {
                        '<' => new P(-1, 0),
                        '>' => new P(1, 0),
                        _ => throw new Exception("Unknown jet direction")
                    };

                    var newPosition = rock.Select(p => p.Add(movement)).ToList();

                    if (newPosition.All(p => p.X >= 0 && p.X <= 6 && !containsRock(p)))
                    {
                        rock = newPosition;
                    }

                    jetIndex = jets.WrapIndex(jetIndex + 1);
                }
                else
                {
                    var rockDown = rock.Select(p => p.AddY(-1)).ToList();

                    if (rockDown.Any(p => containsRock(p) || p.Y < 0))
                    {
                        rock.ForEach(p =>
                        {
                            if (!rocksAtRest.ContainsKey(p.Y))
                            {
                                rocksAtRest.Add(p.Y, new HashSet<P>());
                            }

                            rocksAtRest[p.Y].Add(p);
                        });
                        break;
                    }

                    rock = rockDown;
                }

                jet = !jet;
            }

            if (rock.OrderByDescending(p => p.Y).First().Y >= height)
            {
                height = rock.OrderByDescending(p => p.Y).First().Y + 1;

                rocksAtRest.Where(kvp => kvp.Key < height - gameWindow).ToList().ForEach(kvp => rocksAtRest.Remove(kvp.Key));
            }
            shapeIndex = shapes.WrapIndex(shapeIndex + 1);

            if (lookForShortcut && numberOfRocks > gameWindow)
            {
                var indexBits = new BitArray(new[] { shapeIndex, jetIndex });
                var rockBits = new BitArray(gameWindow * 7);
                rocksAtRest
                    .Values
                    .ToList()
                    .ForEach(hs =>
                    {
                        foreach (var p in hs)
                        {
                            rockBits[((height - p.Y - 1) * 7) + p.X] = true;
                        }
                    });

                var sb = new StringBuilder();

                foreach (var array in new[] { indexBits, rockBits })
                {
                    foreach (var bit in array)
                    {
                        sb.Append((bool)bit ? "1" : "0");
                    }
                }

                var hash = sb.ToString();

                if (states.ContainsKey(hash))
                {
                    var deltaY = height - states[hash].height;
                    var deltaRocks = states[hash].rocks - numberOfRocks;

                    heightAdjust += (numberOfRocks / deltaRocks) * deltaY;
                    numberOfRocks %= deltaRocks;

                    lookForShortcut = false;
                }
                else
                {
                    states.Add(hash, (numberOfRocks, height));
                }
            }
        }

        return height + heightAdjust;
    }

    Console.WriteLine($"{inputFile}\n");

    var part1 = getTowerHeight(2022);
    
    Console.WriteLine($"Part 1: {part1}");

    var part2 = getTowerHeight(1000000000000);
    
    Console.WriteLine($"Part 2: {part2}\n");
}
