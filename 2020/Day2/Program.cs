using System.Text.RegularExpressions;
using AoC.Shared.Strings;

Console.WriteLine("AOC - Day 2\n\n");

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    Console.WriteLine($"[{inputFile}]\n");

    var inputRegEx = new Regex(@"(\d+)-(\d+) (.): (.+)");

    var input = File.ReadAllLines(inputFile)
        .Where(line => !string.IsNullOrEmpty(line))
        .Select(line =>
        {
            var match = inputRegEx.Match(line);

            return (
                pos1: match.Groups[1].Value.ToInt32(),
                pos2: match.Groups[2].Value.ToInt32(),
                character: match.Groups[3].Value[0],
                password: match.Groups[4].Value);
        })
        .ToList();

    var part1 = input
        .Where(t =>
        {
            var charCount = t.password.Where(c => c == t.character).Count();
            return t.pos1 <= charCount && charCount <= t.pos2;
        })
        .Count();

    var part2 = input
        .Where(t => new[] { t.password[t.pos1 - 1], t.password[t.pos2 - 1] }
            .Where(c => c == t.character)
            .Count() == 1)
        .Count();

    Console.WriteLine($"Part 1: {part1}");
    Console.WriteLine($"Part 2: {part2}\n");
}
