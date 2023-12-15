using System.Text.RegularExpressions;
using AoC.Shared.Collections;
using AoC.Shared.Strings;

Console.WriteLine("AOC - Day 15\n\n");

static int hash(string str)
{
    var val = 0;

    foreach (var c in str)
    {
        val += c;
        val *= 17;
        val %= 256;
    }

    return val;
}

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    Console.WriteLine($"[{inputFile}]\n");

    var steps = File.ReadAllLines(inputFile)
        .Where(line => !string.IsNullOrEmpty(line))
        .Single()
        .Split(",", StringSplitOptions.RemoveEmptyEntries)
        .Select(step =>
        {
            var match = new Regex(@"(.+)([=-])(.*)").Match(step);
            var operation = match.Groups[2].Value.Single();

            return (
                step,
                label: match.Groups[1].Value,
                operation,
                focalLength: operation == '=' ? match.Groups[3].Value.ToInt32() : 0
            );
        })
        .ToList();

    Console.WriteLine($"Part 1: {steps.Select(step => hash(step.step)).Sum()}");

    var boxes = new Dictionary<int, List<(string label, int focalLength)>>();
    foreach (var boxId in Enumerable.Range(0, 256))
    {
        boxes.Add(boxId, new List<(string, int)>());
    }

    foreach (var step in steps)
    {
        var box = boxes[hash(step.label)];

        if (step.operation == '-')
        {
            box.RemoveFirst(t => t.label == step.label);
        }
        else
        {
            var lens = (step.label, step.focalLength);
            var i = box.IndexOf(t => t.label == step.label);

            if (i != -1)
            {
                box.ReplaceAt(i, lens);
            }
            else
            {
                box.Add(lens);
            }
        }
    }

    var part2 = boxes
        .SelectMany(kvp => kvp.Value
            .Select((lens, s) => (1 + kvp.Key) * (s + 1) * lens.focalLength))
        .Sum();

    Console.WriteLine($"Part 2: {part2}\n");
}
