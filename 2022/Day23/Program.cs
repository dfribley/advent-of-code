using AoC.Shared.Strings;
using Shared.Collections;
using Shared.Enumerable;
using System.Numerics;

Console.WriteLine("AOC - Day 23\n");

var N = new Vector2(-1, 0);
var NE = new Vector2(-1, 1);
var E = new Vector2(0, 1);
var SE = new Vector2(1, 1);
var S = new Vector2(1, 0);
var SW = new Vector2(1, -1);
var W = new Vector2(0, -1);
var NW = new Vector2(-1, -1);

var directions = new[] { N, NE, E, SE, S, SW, W, NW };
var considerations = new[]
{
    new [] { N, NE, NW },
    new [] { S, SE, SW },
    new [] { W, NW, SW },
    new [] { E, NE, SE },
};

foreach (var input in new[] { "sample.txt", "input.txt" })
{
    // Elf positioning system
    var eps = File.ReadLines(input)
        .SelectMany((line, row) =>
        {
            var i = line.AllIndexesOf("#");

            return i.Select(col => new Vector2(row, col));
        })
        .ToHashSet();

    Console.WriteLine($"[{input}]\n");

    int moveTheElves(HashSet<Vector2> elves, Func<int, int, bool> endState)
    {
        var rounds = 0;
        var maxRounds = 1000000;
        var considerationIndex = 0;
        var moves = new Dictionary<Vector2, IList<Vector2>>();

        while (++rounds < maxRounds)
        {
            foreach (var elf in elves)
            {
                if (directions.Any(d => elves.Contains(elf + d)))
                {
                    _ = considerations.StartAt(considerationIndex).Any(aps =>
                    {
                        if (aps.All(d => !elves.Contains(elf + d)))
                        {
                            var target = elf + aps[0];

                            if (!moves.ContainsKey(target))
                            {
                                moves.Add(target, new List<Vector2>());
                            }

                            moves[target].Add(elf);
                            return true;
                        }

                        return false;
                    });
                }
            }

            // Second half
            moves = new Dictionary<Vector2, IList<Vector2>>(moves.Where(kvp => kvp.Value.Count == 1));

            foreach (var move in moves)
            {
                elves.Remove(move.Value[0]);
                elves.Add(move.Key);
            }

            considerationIndex = considerations.WrapIndex(considerationIndex + 1);

            if (endState(rounds, moves.Count))
            {
                return rounds;
            }

            moves.Clear();
        }

        throw new Exception($"Simulation exceed max rounds of {maxRounds}");
    };

    var epsCopy = new HashSet<Vector2>(eps);
    _ = moveTheElves(epsCopy, (rounds, moves) => rounds == 10);

    var rows = epsCopy.Select(elf => elf.X);
    var cols = epsCopy.Select(elf => elf.Y);

    var part1 = ((int)rows.Max() - (int)rows.Min() + 1) * ((int)cols.Max() - (int)cols.Min() + 1) - epsCopy.Count;
    Console.WriteLine($"Part 1: {part1}");

    var part2 = moveTheElves(eps, (rounds, moves) => moves == 0);
    Console.WriteLine($"Part 2: {part2}\n");
}
