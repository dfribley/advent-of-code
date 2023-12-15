using AoC.Shared.Grid;
using AoC.Shared.Looping;
using AoC.Shared.Points;

// Cycle shortcut - known states
Console.WriteLine("AOC - Day 14\n\n");

foreach (var inputFile in new[] { "sample.txt", "input.txt"})
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    Console.WriteLine($"[{inputFile}]\n");

    var input = File.ReadAllLines(inputFile)
        .Where(line => !string.IsNullOrEmpty(line))
        .ToGrid();

    int spinCycle(Grid field, int cycles, int rounds)
    {
        var states = new Dictionary<string, int>();
        var skipped = false;

        while (rounds-- > 0)
        {
            cycles.Loop((i) =>
            {
                var rocks = field
                    .Where(c => c == 'O')
                    .OrderByDescending(r => r.Y)
                    .ToList();

                rocks.ForEach(rock =>
                {
                    while (true)
                    {
                        var newPosition = rock.AddY(1);

                        if (field.IsValid(newPosition) && field[newPosition] == '.')
                        {
                            field[rock] = '.';
                            field[newPosition] = 'O';

                            rock = newPosition;
                        }
                        else
                        {
                            break;
                        }
                    }
                });

                if (!skipped)
                {
                    var state = string.Concat(field.rows);
                    if (states.ContainsKey(state))
                    {
                        var cycle = states[state] - rounds;
                        rounds %= cycle;

                        skipped = true;
                    }
                    else
                    {
                        states.Add(state, rounds);
                    }
                }

                if (cycles > 1)
                {
                    field.Rotate90();
                }
            });
        }

        return field.Where(c => c == 'O').Select(r => r.Y + 1).Sum();
    }

    Console.WriteLine($"Part 1: {spinCycle(input.Clone(), 1, 1)}");
    Console.WriteLine($"Part 2: {spinCycle(input.Clone(), 4, 1000000000)}\n");
}
