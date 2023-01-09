using AoC.Shared.Ranges;
using AoC.Shared.Strings;
using System.Data;

Console.WriteLine("AOC - Day 4\n\n");

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    Console.WriteLine($"[{inputFile}]\n");

    var assignments = File
        .ReadAllLines(inputFile)
        .Where(l => !string.IsNullOrEmpty(l))
        .Select(l =>
        {
            var parts = l.Split(',');
            var range1 = parts[0].Split('-');
            var range2 = parts[1].Split('-');

            return (
                Assignment1: new Range(range1[0].ToInt32(), range1[1].ToInt32()),
                Assignment2: new Range(range2[0].ToInt32(), range2[1].ToInt32())
            );
        })
        .ToList();

    var part1 = assignments
        .Where(a => a.Assignment1.FullyContains(a.Assignment2) || a.Assignment2.FullyContains(a.Assignment1))
        .Count();
    
    Console.WriteLine($"Part 1: {part1}");

    var part2 = assignments
        .Where(a => a.Assignment1.Overlaps(a.Assignment2) || a.Assignment2.Overlaps(a.Assignment1))
        .Count();
    
    Console.WriteLine($"Part 2: {part2}\n");
}