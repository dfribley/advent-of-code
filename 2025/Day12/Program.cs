// Advent of Code challenge: https://adventofcode.com/2025/day/12

using System.Drawing;
using AoC.Shared.Enumerable;
using AoC.Shared.Grid;
using AoC.Shared.Looping;
using AoC.Shared.Points;
using AoC.Shared.Strings;

Console.WriteLine("AoC - Day 12\n\n");

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    Console.WriteLine($"[{inputFile}]\n");

    var input = File.ReadAllLines(inputFile)
        .Split(string.IsNullOrEmpty)
        .ToList();
    
    var shapes = input.SkipLast(1)
        .Select(grp =>
        {
            var i = grp.Values.First().Split(":")[0].ToInt32();
            var present = grp.Values.Skip(1).ToGrid();
            
            var options = new List<HashSet<Point>>();
            options.Add(present.Where(c => c == '#').ToHashSet());

            present.Rotate90();
            options.Add(present.Where(c => c == '#').ToHashSet());
            
            present.Rotate90();
            options.Add(present.Where(c => c == '#').ToHashSet());
            
            present.Rotate90();
            options.Add(present.Where(c => c == '#').ToHashSet());

            for (var j = 3; j > 0; j--)
            {
                if (options.Take(j).Any(o => o.SetEquals(options[j])))
                {
                    options.RemoveAt(j);
                }
            }
            
            return (i, options);
        })
        .ToDictionary(t => t.i, t => (points: t.options, blocks: t.options.First().Count));

    var grids = input.Last().Values
        .Select(line =>
        {
            var parts = line.Split(": ");
            var sizes = parts[0].Split('x');
            var shapes = parts[1].Split(' ')
                .Select((num, i) => (i, num))
                .ToDictionary(t => t.i, t => t.num.ToInt32());

            return (size: (wide: sizes[0].ToInt32(), lng: sizes[1].ToInt32()), shapes);
        })
        .ToList();

    var part1 = grids.Count(g => CanFitPresents(g.size, g.shapes));
    
    Console.WriteLine($"Part 1: {part1}\n");

    continue;

    bool CanFitPresents((int w, int l) size, Dictionary<int, int> requirements)
    {
        Console.Write(".");
        var totalPoints = size.l * size.w;
        var requiredPoints = requirements.Sum(kvp => shapes[kvp.Key].blocks * kvp.Value);

        if (requiredPoints > totalPoints)
        {
            return false;
        }

        //return true;
        
        var pieces = requirements.SelectMany(kvp =>
        {
            var pieces = new List<List<HashSet<Point>>>();
            
            kvp.Value.Loop(i =>
            {
                pieces.Add(shapes[kvp.Key].points);
            });
            
            return pieces;
        }).ToList();
        
        var grid = new List<string>();

        for (var p = 0; p < size.l; p++)
        {
            grid.Add(new string('.', size.w));
        }
        
        return Solve(grid.ToGrid(), pieces, 0);
    }

    bool Solve(Grid grid, List<List<HashSet<Point>>> pieces, int i)
    {
        if (i == pieces.Count)
        {
            return true;
        }

        foreach (var config in pieces[i])
        {
            foreach (var pos in grid.Where(c => c == '.'))
            {
                if (config.All(p => grid.IsValid(pos.Add(p)) && grid[pos.Add(p)] == '.'))
                {
                    foreach (var p in config)
                    {
                        grid[pos.Add(p)] = '#';
                    }
                    
                    if (Solve(grid, pieces, i + 1))
                    {
                        return true;
                    }
                    
                    foreach (var p in config)
                    {
                        grid[pos.Add(p)] = '.';
                    }
                }
            }
        }

        return false;
    }
}
