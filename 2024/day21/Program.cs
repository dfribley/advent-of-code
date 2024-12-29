// Advent of Code challenge: https://adventofcode.com/2024/day/21

using System.Text;
using AoC.Shared.Grid;
using AoC.Shared.Strings;

Console.WriteLine("AoC - Day 21\n\n");

var np = new[] { "789", "456", "123", " 0A" }.ToGrid();
var dp = new[] { " ^A", "<v>" }.ToGrid();

var knownSequenceLengths = new Dictionary<(string, int), long>();

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    Console.WriteLine($"[{inputFile}]\n");

    var codes = File.ReadAllLines(inputFile)
        .Where(line => !string.IsNullOrEmpty(line))
        .Select(code =>  (code, dpSequence: GetSubSequence(code, np)))
        .ToArray();
    
    var part1 = codes
        .Select(t => GetLengthOfSequence(t.dpSequence, 2) * t.code[..^1].ToInt32())
        .Sum();

    Console.WriteLine($"Part 1: {part1}");
    
    var part2 = codes
        .Select(t => GetLengthOfSequence(t.dpSequence, 25) * t.code[..^1].ToInt32())
        .Sum();
    
    Console.WriteLine($"Part 2: {part2}\n");
}

return;

long GetLengthOfSequence(string sequence, int dirRobots)
{
    var key = (sequence, dirRobots);
    if (knownSequenceLengths.TryGetValue(key, out var known))
    {
        return known;
    }

    if (dirRobots == 0)
    {
        return sequence.Length;
    }
    
    knownSequenceLengths[key] = sequence
        .Split('A')
        .SkipLast(1)
        .Select(chunk => GetLengthOfSequence(GetSubSequence($"{chunk}A", dp), dirRobots - 1))
        .Sum();

    return knownSequenceLengths[key];
}

string GetSubSequence(string sequence, Grid pad)
{
    var subSequence = new StringBuilder();
    var blank = pad.First(chr => chr == ' ');
    var previous = pad.First(chr => chr == 'A');

    foreach (var key in sequence)
    {
        var start = previous;
        var end = pad.First(chr => chr == key);

        var vertical = end.Y > start.Y ? new string('^', end.Y - start.Y) : new string('v', start.Y - end.Y);
        var horizontal = end.X > start.X ? new string('>', end.X - start.X) : new string('<', start.X - end.X);

        if (blank.Y == start.Y && blank.X == end.X)
        {
            subSequence.Append(vertical + horizontal);
        }
        else if (blank.X == start.X && blank.Y == end.Y)
        {
            subSequence.Append(horizontal + vertical);
        }
        else if (end.X < start.X)
        {
            subSequence.Append(horizontal + vertical);
        }
        else
        {
            subSequence.Append(vertical + horizontal);
        }
        
        subSequence.Append('A');
        previous = end;
    }

    return subSequence.ToString();
}
