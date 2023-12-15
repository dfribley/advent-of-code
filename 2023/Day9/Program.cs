using AoC.Shared.Strings;

Console.WriteLine("AOC - Day 9\n\n");

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    Console.WriteLine($"[{inputFile}]\n");

    var input = File.ReadAllLines(inputFile)
        .Where(line => !string.IsNullOrEmpty(line))
        .Select(line => line
            .Split(" ", StringSplitOptions.RemoveEmptyEntries)
            .Select(str => str.ToInt32())
            .ToArray());

    static int nextVal(int[] values)
    {
        if (values.All(v => v == 0))
        {
            return 0;
        }

        var newValues = new List<int>();

        for (int i = 0; i < values.Length - 1; i++)
        {
            newValues.Add(values[i + 1] - values[i]);
        }

        return values[^1] + nextVal(newValues.ToArray());
    }

    var part1 = input.Select(h => nextVal(h)).Sum();
    Console.WriteLine($"Part 1: {part1}");

    var part2 = input.Select(h => nextVal(h.Reverse().ToArray())).Sum();
    Console.WriteLine($"Part 2: {part2}\n");
}
