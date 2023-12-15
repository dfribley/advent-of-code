using AoC.Shared.Distance;
using AoC.Shared.Grid;

Console.WriteLine("AOC - Day 11\n\n");

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    Console.WriteLine($"[{inputFile}]\n");

    var space = File.ReadAllLines(inputFile)
        .Where(line => !string.IsNullOrEmpty(line))
        .ToGrid();

    var rowsToExpand = space.GetYWhere(vals => vals.All(v => v == '.'));
    var colsToExpand = space.GetXWhere(vals => vals.All(v => v == '.'));

    var galaxies = space.Where(val => val == '#');
    var pairs = galaxies.SkipLast(1)
        .SelectMany((galaxy, i) => galaxies.Skip(i + 1).Select(target => (galaxy, target)));

    long expandAndSumPaths(int expandFactor)
    {
        return pairs
            .Select(p =>
            {
                var yplus = rowsToExpand.Count(r => Math.Min(p.galaxy.Y, p.target.Y) <= r && r <= Math.Max(p.galaxy.Y, p.target.Y));
                var xplus = colsToExpand.Count(c => Math.Min(p.galaxy.X, p.target.X) <= c && c <= Math.Max(p.galaxy.X, p.target.X));

                return (long)TaxiCab.GetDistance(p.galaxy, p.target) + ((expandFactor - 1) * (yplus + xplus));
            })
            .Sum();
    }

    Console.WriteLine($"Part 1: {expandAndSumPaths(2)}");
    Console.WriteLine($"Part 2: {expandAndSumPaths(1000000)}\n");
}
