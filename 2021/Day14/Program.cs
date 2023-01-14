using Shared.Enumerable;

Console.WriteLine("AOC - Day 14\n\n");


foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    Console.WriteLine($"[{inputFile}]\n");

    var input = File.ReadAllLines(inputFile)
        .Split(string.IsNullOrEmpty)
        .ToList();
    var template = input[0].Values.First();
    var rules = input[1].Values
        .Select(line =>
        {
            var parts = line.Split(" -> ");

            return (pattern: parts[0], insert: parts[1]);
        })
        .ToDictionary(t => t.pattern, t => t.insert);


    var rounds = 40;

    while (rounds-- > 0)
    {
        for (var i = template.Length - 1; i > 0; i--)
        {
            var pair = template.Substring(i - 1, 2);

            if (rules.ContainsKey(pair))
            {
                template = template.Insert(i, rules[pair]);
            }
        }
    }

    var counts = template.GroupBy(c => c).Select(g => g.LongCount()).Order().ToList();

    Console.WriteLine($"Part 1: {counts.Last() - counts.First()}");

    Console.WriteLine("Part 2:\n");
}