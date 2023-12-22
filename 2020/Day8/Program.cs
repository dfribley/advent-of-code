using AoC.Shared.Strings;

Console.WriteLine("AOC - Day 8\n\n");

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    Console.WriteLine($"[{inputFile}]\n");

    var bootCode = File.ReadAllLines(inputFile)
        .Where(line => !string.IsNullOrEmpty(line))
        .Select(line =>
        {
            var parts = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            return (operation: parts[0], argument: parts[1].ToInt32());
        })
        .ToList();

    static (bool success, int result) runCode(IList<(string, int)> code)
    {
        var result = 0;
        var seen = new HashSet<int>();

        for (int i = 0; i < code.Count;)
        {
            if (seen.Contains(i))
            {
                return (false, result);
            }

            seen.Add(i);

            var (operation, argument) = code[i];

            if (operation == "acc")
            {
                result += argument;
            }

            if (operation == "jmp")
            {
                i += argument;
            }
            else
            {
                i++;
            }
        }

        return (true, result);
    }

    Console.WriteLine($"Part 1: {runCode(bootCode).result}");

    for (int i = 0; i < bootCode.Count; i++)
    {
        if (bootCode[i].operation == "acc")
        {
            continue;
        }

        var newCode = new List<(string operation, int argument)>(bootCode);
        newCode[i] = (newCode[i].operation == "nop" ? "jmp" : "nop", newCode[i].argument);

        var (success, result) = runCode(newCode);

        if (success)
        {
            Console.WriteLine($"Part 2: {result}\n");
            break;
        }
    }
}
