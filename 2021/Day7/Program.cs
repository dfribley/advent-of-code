using AoC.Shared.Strings;

Console.WriteLine("AOC - Day 7\n\n");

static int getPart2FuelCost(int distance)
{
    return distance * (distance + 1) / 2;
}

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    Console.WriteLine($"[{inputFile}]\n");

    var numbers = File.ReadAllText(inputFile)
        .Trim()
        .Split(',')
        .Select(n => n.ToInt32())
        .Order()
        .ToList();

    var mid = numbers.Count / 2;
    var median = (double)numbers[mid];

    if (numbers.Count % 2 == 0)
    {
        median = (median + numbers[mid - 1]) / 2.0;
    }

    var part1 = numbers.Select(n => Math.Abs(median - n)).Sum();
    Console.WriteLine($"Part 1: {part1}");

    var part2 = int.MaxValue;
    for (var i = 0; true; i++)
    {
        var score = numbers.Select(n => getPart2FuelCost(Math.Abs(n - i))).Sum();

        if (score > part2)
        {
            break;
        }

        part2 = score;
    }
    Console.WriteLine($"Part 2: {part2}\n");
}