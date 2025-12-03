// Advent of Code challenge: https://adventofcode.com/2025/day/2

using AoC.Shared.Enumerable;
using AoC.Shared.Ranges;

Console.WriteLine("AoC - Day 2\n\n");

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    Console.WriteLine($"[{inputFile}]\n");

    var ranges = File.ReadAllText(inputFile)
        .Split(',')
        .Select(Range64.Parse)
        .ToList();

    var part1 = ranges
        .SelectMany(r => r
            .Where(num =>
            {   
                var numStr = num.ToString();
                return numStr.Length % 2 == 0 && IsInvalidIdString(numStr, numStr.Length / 2);
            })
            .ToList()
        )
        .Sum();

    Console.WriteLine($"Part 1: {part1}");
    
    var part2 = ranges
        .SelectMany(r => r
            .Where(IsInvalidId)
            .ToList()
        )
        .Sum();
    
    Console.WriteLine($"Part 2: {part2}\n");
}

return;

static bool IsInvalidId(long num)
{
    var str = num.ToString();
    var chunk = str.Length / 2;

    while (chunk > 0)
    {
        if (str.Length % chunk == 0 && IsInvalidIdString(str, chunk))
        {
            return true;
        }
        
        chunk--;
    }

    return false;
}

static bool IsInvalidIdString(string idAsString, int sequenceLength)
{
    var pattern = idAsString[..sequenceLength];

    for (var i = 0; i < idAsString.Length; i += sequenceLength)
    {
        if (i + sequenceLength > idAsString.Length || idAsString.Substring(i, sequenceLength) != pattern)
        {
            return false;
        }
    }

    return true;
}