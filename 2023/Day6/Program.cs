using AoC.Shared.Enumerable;
using AoC.Shared.Strings;

// Quadratic Formula
Console.WriteLine("AOC - Day 6\n\n");

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    Console.WriteLine($"[{inputFile}]\n");

    var input = File.ReadAllLines(inputFile)
        .Where(line => !string.IsNullOrEmpty(line))
        .ToList();

    // Brute force with Log(n)
    static int findWinConditions(long time, long distance)
    {
        var wins = 0;

        for (int button = 0; button <= time; button++)
        {
            if ((time - button) * button > distance)
            {
                wins++;
            }
        }

        return wins;
    }

    var times = input[0].Split(" ", StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(i => i.ToInt32());
    var distances = input[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(i => i.ToInt32()).ToList();

    var part1 = times
        .Select((t, i) => findWinConditions(t, distances[i]))
        .Product();

    Console.WriteLine($"Part 1: {part1}");

    var time = new string(input[0].Where(char.IsDigit).ToArray()).ToInt32();
    var distance = new string(input[1].Where(char.IsDigit).ToArray()).ToInt64();

    Console.WriteLine($"Part 2: {findWinConditions(time, distance)}\n");
}
