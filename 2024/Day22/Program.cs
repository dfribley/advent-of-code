// Advent of Code challenge: https://adventofcode.com/2024/day/22

Console.WriteLine("AoC - Day 22\n\n");

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    Console.WriteLine($"[{inputFile}]\n");

    var evolvedNumbers = File.ReadAllLines(inputFile)
        .Where(line => !string.IsNullOrEmpty(line))
        .Select(int.Parse)
        .Select(sn => EvolveNumber(sn, 2000))
        .ToArray();
    
    Console.WriteLine($"Part 1: {evolvedNumbers.Select(n => n.number).Sum()}");

    var sequencePrices = new Dictionary<string, int>();
    
    foreach (var (_, changes) in evolvedNumbers)
    {
        var prices = new Dictionary<string, int>();
        var price = changes[0] + changes.Skip(1).Take(3).Sum();

        for (var i = 4; i < changes.Length; i++)
        {
            var key = string.Join("", changes.Skip(i - 3).Take(4).ToArray());
            price += changes[i];

            if (!prices.ContainsKey(key))
            {
                prices[key] = price;
            }
        }
        
        foreach (var (key, value) in prices)
        {
            if (!sequencePrices.ContainsKey(key))
            {
                sequencePrices[key] = value;
            }
            else
            {
                sequencePrices[key] += value;
            }
        }
    }
    
    Console.WriteLine($"Part 2: {sequencePrices.Max(kvp => kvp.Value)}\n");
}

return;

(long number, int[] changes) EvolveNumber(long secret, int i)
{
    var last = (int)secret % 10;
    var changes = new List<int> { last };
    
    while (i-- > 0)
    {
        secret = MixAndPrune(secret, secret * 64);
        secret = MixAndPrune(secret, (long) Math.Floor(secret / 32.0));
        secret = MixAndPrune(secret, secret * 2048);
        
        var lastDigit = (int)secret % 10;
        changes.Add(lastDigit - last);
        last = lastDigit;
    }

    return (secret, changes.ToArray());
}

long MixAndPrune(long secret, long mixVal)
{
    secret ^= mixVal;
    return secret % 16777216;
}
