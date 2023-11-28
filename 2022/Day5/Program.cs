using System.Text.RegularExpressions;
using AoC.Shared.Enumerable;
using AoC.Shared.Strings;

Console.WriteLine("AOC - Day 5\n\n");

var stackValRegEx = new Regex(@"\[[A-Z]]");
var moveRegEx = new Regex(@"move (\d+) from (\d+) to (\d+)");

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    Console.WriteLine($"[{inputFile}]\n");

    var input = File
        .ReadAllLines(inputFile)
        .Split(string.IsNullOrEmpty)
        .ToList();
    var stacks = new Dictionary<int, string>();

    foreach (var line in input[0].Values.Reverse())
    {
        var match = stackValRegEx.Match(line);

        while (match.Success)
        {
            var stack = match.Index / 4 + 1;

            if (!stacks.ContainsKey(stack))
            {
                stacks.Add(stack, string.Empty);
            }

            stacks[stack] += match.Value[1];
            match = match.NextMatch();
        }
    }

    var moves = input[1].Values
        .Where(line => moveRegEx.IsMatch(line))
        .Select(line =>
        {
            var match = moveRegEx.Match(line);

            return (
                amount: match.Groups[1].Value.ToInt32(),
                source: match.Groups[2].Value.ToInt32(),
                destination: match.Groups[3].Value.ToInt32()
            );
        });

    var stacksCopy = new Dictionary<int, string>(stacks);
    foreach (var (amount, source, destination) in moves)
    {
        stacksCopy[destination] += string.Concat(stacksCopy[source][^amount..].Reverse());
        stacksCopy[source] = stacksCopy[source][..^amount];
    }
    var part1 = string.Concat(stacksCopy.Values.Select(s => s.Last()));
    
    Console.WriteLine($"Part 1: {part1}");

    foreach (var (amount, source, destination) in moves)
    {
        stacks[destination] += stacks[source][^amount..];
        stacks[source] = stacks[source][..^amount];
    }
    var part2 = string.Concat(stacks.Values.Select(s => s.Last()));
    
    Console.WriteLine($"Part 2: {part2}");
}