// Advent of Code challenge: https://adventofcode.com/2019/day/4

using AoC.Shared.Strings;

Console.WriteLine("AoC - Day 4\n\n");

foreach (var inputFile in new[] { /* "sample.txt", */ "input.txt" })
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    Console.WriteLine($"[{inputFile}]\n");

    var input = File.ReadAllText(inputFile)
        .Split('-')
        .ToList();
    var min = input[0].ToInt32();
    var max = input[1].ToInt32();
    var range = Enumerable.Range(min, max - min + 1);

    var part1 = range.Count(pw => IsValidPassword(pw, gs => gs >= 2));
    Console.WriteLine($"Part 1: {part1}");
    
    var part2 = range.Count(pw => IsValidPassword(pw, gs => gs == 2));
    Console.WriteLine($"Part 2: {part2}\n");
}

return;

static bool IsValidPassword(int password, Func<int, bool> groupSizeValidFunction)
{
    var passwordStr = password.ToString();
    var counts = new Dictionary<int, int>();
    
    for (var i = 0; i < passwordStr.Length; i++)
    {
        var me = passwordStr[i].ToInt32();
        counts[me] = counts.GetValueOrDefault(me) + 1;

        if (i == passwordStr.Length - 1)
        {
            break;
        }

        if (me > passwordStr[i + 1].ToInt32())
        {
            return false;
        }
    }

    return counts.Values.Any(groupSizeValidFunction);
}