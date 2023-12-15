using AoC.Shared.Strings;

Console.WriteLine("AOC - Day 6\n\n");

static long modelGrowth(IDictionary<int, long> fish, int days)
{
    while (days-- > 0)
    {
        var spawning = fish[0];

        for (var i = 1; i < 9; i++)
        {
            fish[i-1] = fish[i];
        }

        fish[6] += spawning;
        fish[8] = spawning;
    }

    return fish.Values.Sum();
}

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    Console.WriteLine($"[{inputFile}]\n");

    var fish = File.ReadAllText(inputFile)
        .Trim()
        .Split(',')
        .Select(n => n.ToInt32())
        .GroupBy(age => age)
        .ToDictionary(g => g.Key, g => (long)g.Count());

    for (var i = 0; i < 9; i++)
    {
        if (!fish.ContainsKey(i))
        {
            fish[i] = 0;
        }
    }

    var part1 = modelGrowth(new Dictionary<int, long>(fish), 80);
    Console.WriteLine($"Part 1: {part1}");

    var part2 = modelGrowth(fish, 256);
    Console.WriteLine($"Part 2: {part2}");
}