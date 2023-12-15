using AoC.Shared.Enumerable;
using AoC.Shared.MathUtils;

// Least Common Multiple
Console.WriteLine("AOC - Day 8\n\n");

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    Console.WriteLine($"[{inputFile}]\n");

    var input = File.ReadAllLines(inputFile)
        .Where(line => !string.IsNullOrEmpty(line))
        .ToList();

    var instructions = input[0];
    var network = input.Skip(1)
        .Select(line =>
        {
            var parts = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            var node = parts[0];
            var left = parts[2].Substring(1, 3);
            var right = parts[3][..3];

            return (node, left, right);
        })
        .ToDictionary(t => t.node, t => (t.left, t.right));

    int stepsToEnd(string node)
    {
        var steps = 0;
        var moves = instructions.AsWrapable();

        while (!node.EndsWith("Z"))
        {
            node = moves.Current == 'L'
                ? network[node].left
                : network[node].right;

            moves.Next();
            steps++;
        }

        return steps;
    }

    Console.WriteLine($"Part 1: {stepsToEnd("AAA")}");

    var part2 = network
        .Where(kvp => kvp.Key.EndsWith('A'))
        .Select(kvp => stepsToEnd(kvp.Key))
        .LCM();
       
    Console.WriteLine($"Part 2: {part2}\n");
}
