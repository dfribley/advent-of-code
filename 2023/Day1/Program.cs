using AoC.Shared.Strings;

Console.WriteLine("AOC - Day 1\n\n");

foreach (var inputFile in new[] {"sample.txt", "input.txt"})
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    Console.WriteLine($"[{inputFile}]\n");

    var input = File.ReadAllLines(inputFile)
        .Where(line => !string.IsNullOrEmpty(line))
        .ToList();

    var part1 = input
        .Select(line => line.Where(Char.IsDigit))
        .Select(d => d.First().ToInt32() * 10 + d.Last().ToInt32())
        .Sum();

    Console.WriteLine($"Part 1: {part1}");

    var numbers = new[] { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
    var part2 = input
        .Select(line =>
        {
            var digits = new List<int>();

            for (var i = 0; i < line.Length; i++)
            {
                if (line[i].IsDigit())
                {
                    digits.Add(line[i].ToInt32());
                }
                else
                {
                    for (var n = 0; n < numbers.Length; n++)
                    {
                        if (line[i..].StartsWith(numbers[n]))
                        {
                            digits.Add(n + 1);
                            break;
                        }
                    }
                }
            }

            return digits;
        })
        .Select(digits => digits.First() * 10 + digits.Last())
        .Sum();

    Console.WriteLine($"Part 2: {part2}\n");
}
