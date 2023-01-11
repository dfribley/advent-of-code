using AoC.Shared.Strings;

Console.WriteLine("AOC - Day 10\n\n");

var pairs = new Dictionary<char, char>
{
    {')', '('},
    {']', '['},
    {'}', '{'},
    {'>', '<'}
};

var pointValues = new Dictionary<char, int>
{
    {'(', 1},
    {'[', 2},
    {'{', 3},
    {'<', 4},
    {')', 3},
    {']', 57},
    {'}', 1197},
    {'>', 25137}
};

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    Console.WriteLine($"[{inputFile}]\n");

    var input = File.ReadAllLines(inputFile)
        .Where(line => !string.IsNullOrEmpty(line))
        .Select(line => {
            var stack = new Stack<char>();
            var score = 0;

            foreach (var c in line)
            {
                if (!pairs.ContainsKey(c))
                {
                    stack.Push(c);
                }
                else if (stack.Pop() != pairs[c])
                {
                    score = pointValues[c];
                    break;
                }
            }

            var totalScore = 0L;
            foreach(var c in stack)
            {
                totalScore = (totalScore * 5) + pointValues[c];
            }

            return (score, totalScore);
        })
        .ToList();

    var part1 = input
        .Select(t => t.score)
        .Sum();

    Console.WriteLine($"Part 1: {part1}");

    var part2 = input
        .Where(t => t.score == 0)
        .OrderBy(t => t.totalScore)
        .ToList();

    Console.WriteLine($"Part 2: {part2[part2.Count / 2].totalScore}\n");
}