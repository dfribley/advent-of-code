// Advent of Code challenge: https://adventofcode.com/2024/day/2

using AoC.Shared.Collections;
using AoC.Shared.Strings;

Console.WriteLine("AoC - Day 2\n\n");

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    Console.WriteLine($"[{inputFile}]\n");

    var input = File.ReadAllLines(inputFile)
        .Where(line => !string.IsNullOrEmpty(line))
        .Select(ln => ln.Split(" ").Select(num => num.ToInt32()).ToArray())
        .ToArray();
    
    Console.WriteLine($"Part 1: {input.Count(IsReportSafe)}");

     var part2 = input
         .Count(report =>
         {
             if (IsReportSafe(report))
             {
                 return true;
             }
             
             for (var i = 0; i < report.Length; i++)
             {
                 if (IsReportSafe(report.RemoveAt(i)))
                 {
                     return true;
                 }
             }

             return false;
         });

    Console.WriteLine($"Part 2: {part2}\n");
}

return;

bool IsReportSafe(int[] values)
{
    var dir  = values[1] > values[0] ? 1 : -1;
        
    for (var i = 1; i < values.Length; i++)
    {
        var delta = values[i] - values[i - 1];
            
        if (delta * dir < 0)
        {
            return false;
        }

        if (Math.Abs(delta) is 0 or > 3)
        {
            return false;
        }
    }

    return true;
}
