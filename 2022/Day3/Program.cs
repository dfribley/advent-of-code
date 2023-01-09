using Shared.Enumerable;

Console.WriteLine("AOC - Day 3\n\n");

var priority = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    Console.WriteLine($"[{inputFile}]\n");

    var rucksacks = File
        .ReadAllLines(inputFile)
        .Where(l => !string.IsNullOrEmpty(l))
        .ToList();

    var part1 = rucksacks
        .Select(l => priority.IndexOf(l[..(l.Length / 2)].Intersect(l[(l.Length / 2)..]).First()) + 1)
        .Sum();
    
    Console.WriteLine($"Part 1: {part1}");

    var part2 = rucksacks
        .Split(3)
        .Select(sg =>
        {
            var sacks = sg.Values.ToList();
            return priority.IndexOf(sacks[0].Intersect(sacks[1]).Intersect(sacks[2]).First()) + 1;
        })
        .Sum();

    Console.WriteLine($"Part 2: {part2}\n");
}