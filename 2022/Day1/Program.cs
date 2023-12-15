using AoC.Shared.Enumerable;
using AoC.Shared.Strings;

Console.WriteLine("AOC - Day 1\n\n");

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    Console.WriteLine($"[{inputFile}]\n");

    var elfLogs = File.ReadLines(inputFile).Split(string.IsNullOrEmpty);

    var part1 = elfLogs
        .Select(l => l.Values.Select(s => s.ToInt32()).Sum())
        .OrderDescending()
        .First();
    Console.WriteLine($"Part 1: {part1}");

    var part2 = elfLogs
        .Select(l => l.Values.Select(s => s.ToInt32()).Sum())
        .OrderDescending()
        .Take(3)
        .Sum();
    Console.WriteLine($"Part 2: {part2}\n");
}