// Advent of Code challenge: https://adventofcode.com/2024/day/13

using System.Drawing;
using System.Text.RegularExpressions;
using AoC.Shared.Enumerable;
using AoC.Shared.Strings;

Console.WriteLine("AoC - Day 13\n\n");

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    Console.WriteLine($"[{inputFile}]\n");

    var btnRegEx = new Regex(@"X\+(\d+), Y\+(\d+)");
    var prizeRegEx = new Regex(@"X=(\d+), Y=(\d+)");
    
    var machines = File.ReadAllLines(inputFile)
        .Split(string.IsNullOrEmpty)
        .Select(grp =>
        {
            var vals = grp.Values.ToArray();
            var aMatch = btnRegEx.Match(vals[0]);
            var bMatch = btnRegEx.Match(vals[1]);
            var prizeMatch = prizeRegEx.Match(vals[2]);
            
            return (a: new Point(aMatch.Groups[1].Value.ToInt32(), aMatch.Groups[2].Value.ToInt32()),
                b: new Point(bMatch.Groups[1].Value.ToInt32(), bMatch.Groups[2].Value.ToInt32()),
                p: new Point(prizeMatch.Groups[1].Value.ToInt32(), prizeMatch.Groups[2].Value.ToInt32()));
        })
        .ToArray();

    var part1 = machines
        .Select(m => FindMinimumCost(m.a.X, m.a.Y, m.b.X, m.b.Y, m.p.X, m.p.Y))
        .Where(c => c != -1)
        .Sum();

    Console.WriteLine($"Part 1: {part1}");
    
    var part2 = machines
        .Select(m => FindMinimumCost(m.a.X, m.a.Y, m.b.X, m.b.Y, m.p.X + 10000000000000, m.p.Y + 10000000000000))
        .Where(c => c != -1)
        .Sum();
    
    Console.WriteLine($"Part 2: {part2}\n");
}

return; 

long FindMinimumCost(int aX, int aY, int bX, int bY, long pX, long pY)
{
    var b = (aX * pY - aY * pX) / (aX * bY - aY * bX);
    var a = (pX - b * bX) / aX;
        
    if (a * aX + b * bX == pX && a * aY + b * bY == pY)
    {
        return 3 * a + b;
    }

    return -1;
}