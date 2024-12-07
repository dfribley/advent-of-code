// Advent of Code challenge: https://adventofcode.com/2024/day/7

using AoC.Shared.Strings;

Console.WriteLine("AoC - Day 7\n\n");

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    Console.WriteLine($"[{inputFile}]\n");

    var equations = File.ReadAllLines(inputFile)
        .Where(line => !string.IsNullOrEmpty(line))
        .Select(ln =>
        {
            var parts = ln.Split(": ");
            var nums = parts[1].Split(" ").Select(int.Parse).ToArray();

            return (test: parts[0].ToInt64(), nums);
        })
        .ToArray();

    var part1 = equations
        .Where(eq => IsTestValid(eq.test, eq.nums[0], eq.nums[1..]))
        .Select(eq => eq.test)
        .Sum();

    Console.WriteLine($"Part 1: {part1}");
    
    var part2 = equations
        .Where(eq => IsTestValid(eq.test, eq.nums[0], eq.nums[1..], true))
        .Select(eq => eq.test)
        .Sum();
    
    Console.WriteLine($"Part 2: {part2}\n");
}

return;

bool IsTestValid(long test, long val, int[] nums, bool part2 = false)
{    
    if (nums.Length == 0)
    {
        return val == test;
    }
    
    if (val > test)
    {
        return false;
    }

    if (IsTestValid(test, val + nums[0], nums[1..], part2))
    {
        return true;
    }
        
    if (IsTestValid(test, val * nums[0], nums[1..], part2))
    {
        return true;
    }
        
    return part2 && IsTestValid(test, $"{val}{nums[0]}".ToInt64(), nums[1..], true);
}
