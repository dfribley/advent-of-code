using AoC.Shared.Strings;

Console.WriteLine("AOC - Day 7\n\n");

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    Console.WriteLine($"[{inputFile}]\n");

    var bags = File.ReadAllLines(inputFile)
        .Where(line => !string.IsNullOrEmpty(line))
        .Select(line =>
        {
            var parts = line.Split(" ", 3, StringSplitOptions.RemoveEmptyEntries);
            var bag = string.Concat(parts.Take(2));
            var contents = line[(line.IndexOf("contain") + 8)..]
                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                .Where(p => !p.StartsWith("no"))
                .Select(p =>
                {
                    var pParts = p.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                    return (amount: pParts[0].ToInt32(), bag: string.Concat(pParts.Skip(1).Take(2)));
                })
                .ToArray();

            return (bag, contents);
        })
        .ToDictionary(b => b.bag, b => b.contents);

    bool containsShinyBag(string bag)
    {
        foreach(var subBag in bags[bag])
        {
            if (subBag.bag == "shinygold" || containsShinyBag(subBag.bag))
            {
                return true;
            }
        }

        return false;
    }

    int countSubBags(string bag)
    {
        var subBags = 0;

        foreach (var subBag in bags[bag])
        {
            subBags += subBag.amount * countSubBags(subBag.bag) + subBag.amount;
        }

        return subBags;
    }

    Console.WriteLine($"Part 1: {bags.Keys.Where(containsShinyBag).Count()}");
    Console.WriteLine($"Part 2: {countSubBags("shinygold")}\n");
}
