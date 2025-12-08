// Advent of Code challenge: https://adventofcode.com/2025/day/8

using System.Numerics;
using AoC.Shared.Distance;
using AoC.Shared.Enumerable;
using AoC.Shared.Strings;

Console.WriteLine("AoC - Day 8\n\n");

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    Console.WriteLine($"[{inputFile}]\n");

    var boxes = File.ReadAllLines(inputFile)
        .Where(line => !string.IsNullOrEmpty(line))
        .Select((line, i) =>
        {
            var parts = line.Split(',');
            return new Vector3(parts[0].ToInt32(), y: parts[1].ToInt32(), z: parts[2].ToInt32());
        })
        .ToList();
    var distances = boxes
        .ToPairs()
        .OrderBy(t => Euclidean.GetDistance(t.a, t.b))
        .ToList();
    var circuits = new List<List<Vector3>>();
    var part1SetSize = inputFile.Contains("sample") ? 10 : 1000;
    
    foreach (var pair in distances.Take(part1SetSize))
    {
        ConnectBoxes(pair.a, pair.b);
    }

    var part1 = circuits
        .Select(c => c.Count)
        .OrderByDescending(c => c)
        .Take(3)
        .Product();
    
    Console.WriteLine($"Part 1: {part1}");
    
    var part2 = 0L;

    foreach (var pair in distances.Skip(part1SetSize))
    {
        ConnectBoxes(pair.a, pair.b);
        
        if (circuits.Count == 1 && circuits.First().Count == boxes.Count)
        {
            part2 = (long) pair.a.X * (long) pair.b.X;
            break;
        }
    }
    
    Console.WriteLine($"Part 2: {part2}\n");
    
    continue;

    void ConnectBoxes(Vector3 a, Vector3 b)
    {
        var aCircuit = circuits.FirstOrDefault(c => c.Contains(a));
        var bCircuit = circuits.FirstOrDefault(c => c.Contains(b));
        
        if (aCircuit == null && bCircuit == null)
        {
            circuits.Add([a, b]);
            return;
        }

        if (aCircuit == bCircuit)
        {
            return;
        }

        if (aCircuit != null && bCircuit != null)
        {
            aCircuit.AddRange(bCircuit);
            circuits.Remove(bCircuit);
            return;
        }

        if (aCircuit != null)
        {
            aCircuit.Add(b);
        }
        else
        {
            bCircuit.Add(a);
        }
    }
}
