using AoC.Shared.Strings;

Console.WriteLine("AOC - Day 13\n\n");

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    Console.WriteLine($"[{inputFile}]\n");

    var input = File.ReadAllLines(inputFile)
        .Where(line => !string.IsNullOrEmpty(line))
        .ToArray();
    
    var earliest = input[0].ToInt32();
    var nextBus = input[1].Split(",")
        .Where(s => s != "x")
        .Select(int.Parse)
        .Select(i => (id: i, wait: i - earliest % i))
        .OrderBy(i => i.wait)
        .First();
    
    Console.WriteLine($"Part 1: {nextBus.id * nextBus.wait}");
    
    var buses = input[1].Split(",")
        .Select((id, index) => (id, index))
        .Where(b => b.id != "x")
        .Select(b => (id: b.id.ToInt32(), b.index))
        .ToArray();

    var timestamp = (long)buses.First().id;
    var step = timestamp;
    
    foreach (var bus in buses.Skip(1))
    {
        while ((timestamp + bus.index) % bus.id != 0)
        {
            timestamp += step;
        }
        
        step *= bus.id;
    }

    Console.WriteLine($"Part 2: {timestamp}\n");
}
