// Advent of Code challenge: https://adventofcode.com/2019/day/1

Console.WriteLine("AoC - Day 1\n\n");

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    Console.WriteLine($"[{inputFile}]\n");

    var modules = File.ReadAllLines(inputFile)
        .Where(line => !string.IsNullOrEmpty(line))
        .Select(int.Parse)
        .ToList();
    
    var part1 = modules.Sum(GetFuelRequiredForMass);
    Console.WriteLine($"Part 1: {part1}");
    
    var part2 = modules.Sum(GetAllFuelRequiredForMass);
    Console.WriteLine($"Part 2: {part2}\n");
}

return;

static int GetAllFuelRequiredForMass(int mass)
{
    var fuel = Math.Max(0, GetFuelRequiredForMass(mass));
    
    return fuel > 0 ? fuel + GetAllFuelRequiredForMass(fuel) : fuel;
}

static int GetFuelRequiredForMass(int mass)
{
    return mass / 3 - 2;
}
