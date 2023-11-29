using AoC.Shared.Enumerable;

Console.WriteLine("AOC - Day 6\n\n");

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    Console.WriteLine($"[{inputFile}]\n");

    var answers = File.ReadAllLines(inputFile)
        .Split(string.IsNullOrEmpty);

    var part1 = answers
        .Select(grp => grp.Values.SelectMany(l => l).Distinct().Count())
        .Sum();

    Console.WriteLine($"Part 1: {part1}");

    var part2 = answers
        .Select(grp => grp.Values.Intersect().Count())
        .Sum();

    Console.WriteLine($"Part 2: { part2 }\n");
}
