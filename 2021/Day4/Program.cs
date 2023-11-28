using AoC.Shared.Enumerable;
using AoC.Shared.Strings;

Console.WriteLine("AOC - Day 4\n\n");

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    Console.WriteLine($"[{inputFile}]\n");

    var input = File.ReadAllLines(inputFile).ToList();

    var numbers = input.First().Split(',').Select(n => n.ToInt32());
    var boards = input.Skip(2).Split(string.IsNullOrEmpty)
        .Select(sg => sg.Values
            .Select(l => l.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(n => n.ToInt32())
                .ToList())
             .ToList())
        .ToList();

    var called = new HashSet<int>();
    var winningScores = new Dictionary<int, int>();

    foreach (var number in numbers)
    {
        called.Add(number);

        for (var i = 0; i < boards.Count; i++)
        {
            if (!winningScores.ContainsKey(i))
            {
                var lines = boards[i].Concat(Enumerable.Range(0, 5).Select(c => boards[i].Select(row => row[c])));

                foreach (var line in lines)
                {
                    if (line.All(called.Contains))
                    {
                        winningScores.Add(i, boards[i].SelectMany(r => r).Where(n => !called.Contains(n)).Sum() * number);
                        break;
                    }
                }
            }
        }

        if (winningScores.Count == boards.Count)
        {
            break;
        }
    }

    Console.WriteLine($"Part 1: {winningScores.First().Value}");
    Console.WriteLine($"Part 2: {winningScores.Last().Value}\n");
}