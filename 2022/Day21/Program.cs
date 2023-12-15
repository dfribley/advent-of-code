using AoC.Shared.Strings;

Console.WriteLine("AOC - Day 21\n");

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    Console.WriteLine($"[{inputFile}]\n");

    var valueMonkeys = new Dictionary<string, long>();
    var mathMonkeys = new Dictionary<string, string[]>();

    double getMonkeyValue(string id, double val)
    {
        if (id == "humn" && val >= 0)
            return val;

        if (valueMonkeys.ContainsKey(id))
        {
            return valueMonkeys[id];
        }

        var parts = mathMonkeys[id];
        var a = getMonkeyValue(parts[0], val);
        var b = getMonkeyValue(parts[2], val);

        return parts[1] switch
        {
            "+" => a + b,
            "-" => a - b,
            "*" => a * b,
            "/" => a / b,
            _ => throw new Exception("Unknown operator")
        };
    };

    File.ReadLines(inputFile)
        .Where(l => !string.IsNullOrEmpty(l))
        .ToList()
        .ForEach(l =>
        {
            var parts = l.Split(' ');

            var id = parts[0][..^1];

            if (parts.Length == 2)
            {
                valueMonkeys.Add(id, parts[1].ToInt32());
            }
            else
            {
                mathMonkeys.Add(id, new[] { parts[1], parts[2], parts[3] });
            }
        });

    var part1 = getMonkeyValue("root", -1);

    Console.WriteLine($"Part 1: {part1}");

    var humnPath = mathMonkeys["root"][0];
    var targetPath = mathMonkeys["root"][2];

    // Assuming humn monkey is only in one path, 
    // assign humn path to path 1 and have path 2
    // be the target result
    if (getMonkeyValue(targetPath, 0) != getMonkeyValue(targetPath, 1))
    {
        // humn monkey is in path 2 - switch
        humnPath = mathMonkeys["root"][2];
        targetPath = mathMonkeys["root"][0];
    }

    // Quick check
    if (getMonkeyValue(targetPath, 0) != getMonkeyValue(targetPath, 1))
    {
        throw new Exception("Humn monkey is in both paths??");
    }

    var change = getMonkeyValue(humnPath, 2) - getMonkeyValue(humnPath, 1);
    var funcDirection = Math.Abs(change) / change;

    var low = 0L;
    var high = long.MaxValue;
    var target = getMonkeyValue(targetPath, 0);

    while (true)
    {
        var mid = (low + high) / 2;
        var difference = (target - getMonkeyValue(humnPath, mid)) * funcDirection;

        if (difference == 0)
        {
            Console.WriteLine($"Part 2: {mid}\n");
            break;
        }

        if (difference < 0)
        {
            high = mid;
        }
        else
        {
            low = mid;
        }
    }
}
