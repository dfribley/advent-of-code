using AoC.Shared.Enumerable;
using AoC.Shared.Ranges;
using AoC.Shared.Strings;

Console.WriteLine("AOC - Day 16\n\n");

foreach (var inputFile in new[] { /*"sample.txt",*/ "input.txt" })
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    Console.WriteLine($"[{inputFile}]\n");

    var input = File.ReadAllLines(inputFile)
        .Split(string.IsNullOrEmpty)
        .ToArray();

    var rules = input[0]
        .Values
        .SelectMany(line =>
        {
            var ranges = line.Split(": ")[1]
                .Split(" or ")
                .Select(range => range.ParseRange());

            return ranges;
        });

    var nearbyTickets = input[2]
        .Values
        .Skip(1)
        .SelectMany(line => line.Split(",").Select(int.Parse));

    var validNearbyTickets = nearbyTickets
        .Where(num => !rules.Any(range => range.Contains(num)))
        .ToArray();

    // Console.WriteLine($"Part 1: {validNearbyTickets.Sum()}");

    var ticketParts = input[1]
        .Values
        .Last()
        .Split(",");
    
    var samples = new List<List<int>>(ticketParts.Length);
    samples.AddRange(ticketParts.Select(t => new List<int> { t.ToInt32() }));

    foreach (var ticket in input[2].Values.Skip(1))
    {
        var parts = ticket.Split(",");
        
        for (var i = 0; i < parts.Length; i++)
        {
            samples[i].Add(parts[i].ToInt32());
        }
    }
    
    var fields = input[0]
        .Values
        .Select(line =>
        {
            var myparts= line.Split(": ");
            var ranges = myparts[1]
                .Split(" or ")
                .Select(range => range.ParseRange());

            return (name: myparts[0], ranges);
        })
        .ToArray();

    var viables = new Dictionary<int, List<int>>();

    for (var i = 0; i < samples.Count; i++)
    {
        viables.Add(i, new List<int>());
    }

    for (var i = 0; i < samples.Count; i++)
    {
        for (var f = 0; f < fields.Length; f++)
        {
            if (samples[i].All(num => fields[f].ranges.First().Contains(num) || fields[f].ranges.Last().Contains(num)))
            {
                viables[i].Add(f);
            }
        }
    }
    
    Console.WriteLine($"Part 2:\n");
}
