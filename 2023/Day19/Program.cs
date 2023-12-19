using System.Data;
using System.Text.RegularExpressions;
using AoC.Shared.Enumerable;
using AoC.Shared.Ranges;
using AoC.Shared.Strings;

Console.WriteLine("AOC - Day 19\n\n");

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    Console.WriteLine($"[{inputFile}]\n");

    var input = File.ReadAllLines(inputFile)
        .Split(string.IsNullOrEmpty)
        .ToList();

    var workflows = input[0].Values
        .Select(wf =>
        {
            var regex = new Regex(@"(.+){(.+)}").Match(wf);
            var ruleParts = regex.Groups[2].Value
                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                .ToList();

            var rules = ruleParts
                .SkipLast(1)
                .Select(r => (
                    cat: r[0],
                    op: r[1],
                    val: r[2..r.IndexOf(':')].ToInt32(),
                    dest: r[(r.IndexOf(':') + 1)..]))
                .ToList();

            return (name: regex.Groups[1].Value, rules, def: ruleParts.Last());
        })
        .ToDictionary(wf => wf.name, wf => wf);

    var parts = input[1].Values
        .Select(p =>
        {
            var match = new Regex(@"{x=(\d+),m=(\d+),a=(\d+),s=(\d+)}").Match(p);

            return new Part(
                x: match.Groups[1].Value.ToInt32(),
                m: match.Groups[2].Value.ToInt32(),
                a: match.Groups[3].Value.ToInt32(),
                s: match.Groups[4].Value.ToInt32());
        })
        .ToArray();

    string screenPart(Part part, string workflow)
    {
        if(workflow == "A" || workflow == "R")
        {
            return workflow;
        }
            
        var (name, rules, def) = workflows[workflow];

        foreach (var rule in rules)
        {
            var val = part.GetValue(rule.cat);

            if (rule.op == '<' && val < rule.val)
            {
                return screenPart(part, rule.dest);
            }

            if (rule.op == '>' && val > rule.val)
            {
                return screenPart(part, rule.dest);
            }
        }

        return screenPart(part, def);
    }

    var part1 = parts
        .Where(part => screenPart(part, "in") == "A")
        .Select(part => part.X + part.M + part.A + part.S)
        .Sum();

    Console.WriteLine($"Part 1: {part1}");

    long screenByRatingsRanges(IDictionary<char, Range> ranges, string workflow)
    {
        if (workflow == "A")
        {
            return ranges.Values.Select(r => (long)r.Count()).Product();
        }

        if (workflow == "R")
        {
            return 0;
        }

        var (_, rules, def) = workflows[workflow];
        var accepted = 0L;

        foreach (var (cat, op, val, dest) in rules)
        {
            var (lowRange, highRange) = ranges[cat].Split(op == '>' ? val : val - 1);
            var ruleRanges = ranges.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            if (op == '>')
            {
                ranges[cat] = lowRange;
                ruleRanges[cat] = highRange;
            }
            else
            {
                ranges[cat] = highRange;
                ruleRanges[cat] = lowRange;
            }

            accepted += screenByRatingsRanges(ruleRanges, dest);
        }

        return accepted += screenByRatingsRanges(ranges, def);
    }

    var part2 = screenByRatingsRanges(
        new Dictionary<char, Range>
        {
            { 'x', new Range(1, 4000) },
            { 'm', new Range(1, 4000) },
            { 'a', new Range(1, 4000) },
            { 's', new Range(1, 4000) }
        },
        "in");

    Console.WriteLine($"Part 2: {part2}\n");
}
