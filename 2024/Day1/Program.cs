using AoC.Shared.Strings;

Console.WriteLine("AOC - Day 1\n\n");

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    Console.WriteLine($"[{inputFile}]\n");

    var input = File.ReadAllLines(inputFile)
        .Where(line => !string.IsNullOrEmpty(line))
        .Select(line =>
        {
            var parts = line
                .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Select(p => p.ToInt32())
                .ToArray();

            return (left: parts[0], right: parts[1]);
        })
        .ToArray();

    var leftIds = input.Select(t => t.left).Order().ToArray();
    var rightIds = input.Select(t => t.right).Order().ToArray();

    var part1 = leftIds
        .Select((lid, i) => Math.Abs(lid - rightIds[i]))
        .Sum();
    Console.WriteLine($"Part 1: {part1}");

    var part2 = leftIds
        .Select(lid => lid * rightIds.Count(rid => rid == lid))
        .Sum();
    Console.WriteLine($"Part 2: {part2}\n");
}
