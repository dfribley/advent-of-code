// Advent of Code challenge: https://adventofcode.com/2025/day/11

using AoC.Shared.Enumerable;

Console.WriteLine("AoC - Day 11\n\n");

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    Console.WriteLine($"[{inputFile}]\n");

    var map = LoadInputFile();
    
    Console.WriteLine($"Part 1: {GetPart1ValidPaths("you")}");

    var knownStates = new Dictionary<(string current, bool seenDac, bool seenFft), long>();
    if (inputFile.Contains("sample"))
    {
        map = LoadInputFile(false);
    }
    
    Console.WriteLine($"Part 2: {GetPart2ValidPaths("svr", false, false)}\n");

    continue;

    int GetPart1ValidPaths(string current)
    {
        return current == "out" ? 1 : map[current].Sum(GetPart1ValidPaths);
    }
    
    long GetPart2ValidPaths(string current, bool seenDac, bool seenFft)
    {
        if (current == "out")
        {
            return seenFft && seenDac ? 1 : 0;
        }
        
        var stateKey = (current, seenDac, seenFft);
        if (knownStates.ContainsKey(stateKey))
        {
            return knownStates[stateKey];
        }

        var validFromHere = map[current]
            .Sum(next => GetPart2ValidPaths(next, seenDac || current == "dac", seenFft || current == "fft"));

        knownStates.Add(stateKey, validFromHere);
        return validFromHere;
    }

    Dictionary<string, string[]> LoadInputFile(bool part1 = true)
    {
        var sections = File.ReadAllLines(inputFile)
            .Where(line => !string.IsNullOrEmpty(line))
            .Split(line => line == "-");

        return (part1 ? sections.First() : sections.Last()).Values
            .Select(line =>
            {
                var parts = line.Split(": ");
        
                return (key: parts[0], value: parts[1].Split(" "));
            })
            .ToDictionary(t => t.key, t => t.value);
    }
}
