using System.Diagnostics;
using System.Numerics;

Console.WriteLine("AOC - Day 24\n");
foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    Console.WriteLine($"[{inputFile}]\n");

    var valley = File.ReadLines(inputFile).ToList();
    var maxRow = valley.Count;
    var maxCol = valley[0].Length;

    var storms = new List<(Vector2 position, Vector2 change)>();
    for (var row = 1; row < valley.Count - 1; row++)
    {
        for (var col = 1; col < valley[0].Length; col++)
        {
            switch (valley[row][col])
            {
                case '^':
                    storms.Add((new Vector2(row - 1, col - 1), new Vector2(-1, 0)));
                    break;
                case '>':
                    storms.Add((new Vector2(row - 1, col - 1), new Vector2(0, 1)));
                    break;
                case 'v':
                    storms.Add((new Vector2(row - 1, col - 1), new Vector2(1, 0)));
                    break;
                case '<':
                    storms.Add((new Vector2(row - 1, col - 1), new Vector2(0, -1)));
                    break;
            };
        }
    }

    // Since storm paths and board size are constant, pre-calculate all storm states
    var stormStates = new Dictionary<int, HashSet<Vector2>>();
    for (int min = 1, maxMin = (valley.Count - 2) * (valley[0].Length - 2); min <= maxMin; min++)
    {
        var time = new Vector2(min, min);
        stormStates.Add(min, new HashSet<Vector2>());

        foreach (var storm in storms)
        {
            var position = storm.position + (storm.change * time);

            // TODO: Use index helper in shared library

            position.X %= (maxRow - 2);
            position.Y %= (maxCol - 2);

            if (position.X < 0)
            {
                position.X = maxRow - 2 + position.X;
            }
            if (position.X > maxRow - 2)
            {
                position.X = position.X - maxRow - 2;
            }
            if (position.Y < 0)
            {
                position.Y = maxCol - 2 + position.Y;
            }
            if (position.Y > maxCol - 2)
            {
                position.Y = position.Y - maxCol - 2;
            }

            stormStates[min].Add(position);
        }
    }

    var start = new Vector2(-1, valley[0].IndexOf('.') - 1);
    var end = new Vector2(maxRow - 2, valley[maxRow - 1].IndexOf('.') - 1);

    var moves = new Queue<(Vector2 position, int minute)>();
    moves.Enqueue((start, 0));

    var enroute = true;
    var leg = 1;

    var sw = new Stopwatch();
    sw.Start();

    while (enroute)
    {
        var move = moves.Dequeue();
        var stormTime = (move.minute % stormStates.Count) + 1;
                
        if (!stormStates[stormTime].Contains(move.position))
        {
            var nextMove = (move.position, move.minute + 1);

            if (!moves.Contains(nextMove))
            {
                moves.Enqueue(nextMove);
            }
        }

        foreach (var moveDirection in new[]
        {
                new Vector2(0, -1),
                new Vector2(1, 0),
                new Vector2(0, 1),
                new Vector2(-1, 0)
            })
        {
            var nextPosition = move.position + moveDirection;

            if ((nextPosition == end && leg == 1) || (nextPosition == start && leg == 2))
            {
                if (nextPosition == end)
                {
                    sw.Stop();
                    Console.WriteLine($"Part 1: ({string.Format("{0:00}:{1:00}.{2:00}", sw.Elapsed.Minutes, sw.Elapsed.Seconds, sw.Elapsed.Milliseconds / 10)}) {move.minute + 1}");
                    sw.Start();
                }

                moves.Clear();
                moves.Enqueue((nextPosition, move.minute + 1));

                leg++;
                break;
            }
            else if (nextPosition == end && leg == 3)
            {
                enroute = false;
                sw.Stop();
                Console.WriteLine($"Part 2: ({string.Format("{0:00}:{1:00}.{2:00}", sw.Elapsed.Minutes, sw.Elapsed.Seconds, sw.Elapsed.Milliseconds / 10)}) {move.minute + 1}\n");
                break;
            }

            if ((nextPosition.X >= 0 && nextPosition.X < maxRow - 2) &&
                (nextPosition.Y >= 0 && nextPosition.Y < maxCol - 2) &&
                !stormStates[stormTime].Contains(nextPosition))
            {
                var nextMove = (nextPosition, move.minute + 1);
                if (!moves.Contains(nextMove))
                {
                    moves.Enqueue(nextMove);
                }
            }
        }
    }
}
