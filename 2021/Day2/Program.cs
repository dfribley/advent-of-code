using AoC.Shared.Strings;
using System.Numerics;

Console.WriteLine("AOC - Day 2\n\n");

foreach (var input in new[] { "sample.txt", "input.txt" })
{
    Console.WriteLine($"[{input}]\n");

    var moves = File.ReadAllLines(input)
        .Select(line =>
        {
            var parts = line.Split(" ");

            return parts[0] switch
            {
                "up" => new Vector2(0, -parts[1].ToInt32()),
                "down" => new Vector2(0, parts[1].ToInt32()),
                "forward" => new Vector2(parts[1].ToInt32(), 0),
                _ => throw new NotImplementedException()
            };
        })
        .ToList();

    var position = new Vector2(0, 0);

    moves.ForEach(move => position += move);
    var part1 = position.X * position.Y;

    Console.WriteLine($"Part 1: {part1}");

    position = new Vector2(0, 0);
    var aim = 0;
    moves.ForEach(move =>
    {
        if (move.X > 0)
        {
            position.X += move.X;
            position.Y += aim * move.X;
        }
        else
        {
            aim += (int)move.Y;
        }
    });

    var part2 = (long)position.X * (long)position.Y;

    Console.WriteLine($"Part 2: {part2}\n");
}
