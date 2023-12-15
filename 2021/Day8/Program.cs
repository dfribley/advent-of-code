using AoC.Shared.Strings;

Console.WriteLine("AOC - Day 8\n\n");

static IDictionary<string, int> getDigits(string[] patterns)
{
    var digits = new Dictionary<int, string>();

    digits[1] = patterns.Single(p => p.Length == 2);
    digits[4] = patterns.Single(p => p.Length == 4);
    digits[7] = patterns.Single(p => p.Length == 3);
    digits[8] = patterns.Single(p => p.Length == 7);

    digits[9] = patterns.Single(p => p.Length == 6 && digits[4].All(p.Contains));
    digits[0] = patterns.Single(p => p.Length == 6 && p != digits[9] && digits[1].All(p.Contains));
    digits[6] = patterns.Single(p => p.Length == 6 && p != digits[9] && p != digits[0]);

    digits[3] = patterns.Single(p => p.Length == 5 && digits[1].All(p.Contains));
    digits[5] = patterns.Single(p => p.Length == 5 && p != digits[3] && p.All(digits[9].Contains));
    digits[2] = patterns.Single(p => p.Length == 5 && p != digits[3] && p != digits[5]);

    return digits.ToDictionary(kvp => new string(kvp.Value.Order().ToArray()), kvp => kvp.Key);
}

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    Console.WriteLine($"[{inputFile}]\n");

    var input = File.ReadAllLines(inputFile)
        .Select(line => {
            var parts = line.Split('|');
            var patterns = parts[0].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var digits = parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);

            return (patterns, digits);
        })
        .ToList();

    var part1 = input
        .Select(t => 
        {
            var digits = getDigits(t.patterns);
            var targets = new [] { 1, 4, 7, 8 };

            return t.digits
                .Select(p => digits[new string(p.Order().ToArray())])
                .Where(targets.Contains)
                .Count();
        })
        .Sum();

    Console.WriteLine($"Part 1: {part1}");

    var part2 = input
        .Select(t =>
        {
            var digits = getDigits(t.patterns);

            return t.digits
                .Select(d => digits[new string(d.Order().ToArray())])
                .Aggregate("", (num, digit) => num += digit)
                .ToInt32();
        })
        .Sum();

    Console.WriteLine($"Part 2: {part2}\n");
}