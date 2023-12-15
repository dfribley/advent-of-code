Console.WriteLine("AOC - Day 12\n\n");

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    Console.WriteLine($"[{inputFile}]\n");

    var tunnels = new Dictionary<string, List<string>>();

    File.ReadAllLines(inputFile)
        .Where(line => !string.IsNullOrEmpty(line))
        .Select(line =>
        {
            var parts = line.Split('-');

            return (start: parts[0], end: parts[1]);
        })
        .ToList()
        .ForEach(t =>
        {
            if (!tunnels.ContainsKey(t.start))
            {
                tunnels[t.start] = new List<string>();
            }

            tunnels[t.start].Add(t.end);

            if (t.start != "start" && t.end != "end")
            {
                if (!tunnels.ContainsKey(t.end))
                {
                    tunnels[t.end] = new List<string>();
                }

                tunnels[t.end].Add(t.start);
            }
        });

    var knownStates = new Dictionary<string, int>();

    int getPathsToEndCount(string path, bool part2)
    {
        var history = path.Split(',');
        var smallCaveRestricted = !part2 || history
                .Where(l => l != "start" && l.All(char.IsLower))
                .GroupBy(l => l)
                .Any(g => g.Count() > 1);

        var stateKey = $"{(smallCaveRestricted ? "1" : "0")}{history.Last()}{history.Length}{history
            .SkipLast(1)
            .Where(l => l != "start" && l.All(char.IsLower))
            .Order()
            .Aggregate("", (me, total) => total += me)}";

        if (knownStates.ContainsKey(stateKey))
        {
            return knownStates[stateKey];
        }

        var endPaths = 0;

        foreach (var option in tunnels[history.Last()])
        {
            if (option == "end")
            {
                endPaths++;
                continue;
            }

            if (option == "start" ||
                (option.All(char.IsLower) && smallCaveRestricted && history.Contains(option)))
            {
                continue;
            }
            
            endPaths += getPathsToEndCount($"{path},{option}", part2);
        }

        knownStates[stateKey] = endPaths;
        return endPaths;
    }

    Console.WriteLine($"Part 1: {getPathsToEndCount("start", false)}");

    Console.WriteLine($"Part 2: {getPathsToEndCount("start", true)}\n");
}