// Advent of Code challenge: https://adventofcode.com/2025/day/3

using AoC.Shared.Strings;

Console.WriteLine("AoC - Day 3\n\n");

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    Console.WriteLine($"[{inputFile}]\n");

    var banks = File.ReadAllLines(inputFile)
        .Where(line => !string.IsNullOrEmpty(line))
        .Select(line => line.Select(c => c.ToInt32()).ToList())
        .ToList();
    
    var part1 = 0;

    foreach (var bank in banks)
    {
        var l1 = 0;

        for (var i = 1; i < bank.Count - 1; i++)
        {
            if (bank[i] > bank[l1])
            {
                l1 = i;
            }
        }

        var l2 = l1 + 1;
        
        for (var i = l2 + 1; i < bank.Count; i++)
        {
            if (bank[i] > bank[l2])
            {
                l2 = i;
            }
        }

        part1 += string.Concat(bank[l1], bank[l2]).ToInt32();
    }
    
    Console.WriteLine($"Part 1: {part1}");

    var part2 = 0L;
    const int maxDigits = 12;
    
    foreach (var bank in banks)
    {
        var toRemove = bank.Count - maxDigits;
        var stack = new Stack<int>();
    
        foreach (var digit in bank)
        {
            while (toRemove > 0 && stack.Count > 0 && stack.Peek() < digit)
            {
                stack.Pop();
                toRemove--;
            }
            stack.Push(digit);
        }
    
        part2 += string.Concat(stack.Reverse().Take(maxDigits)).ToInt64();
    }
    
    Console.WriteLine($"Part 2: {part2}\n");
}
