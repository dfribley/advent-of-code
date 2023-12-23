using AoC.Shared.Strings;

Console.WriteLine("AOC - Day 9\n\n");

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    Console.WriteLine($"[{inputFile}]\n");

    var data = File.ReadAllLines(inputFile)
        .Where(line => !string.IsNullOrEmpty(line))
        .Select(line => line.ToInt64())
        .ToList();

    var preambleLength = inputFile.StartsWith("sample") ? 5 : 25;

    long findInvalidNumber()
    {
        for (int i = preambleLength; i < data.Count; i++)
        {
            var number = data[i];
            var isValid = false;

            for (int lower = i - preambleLength; lower < i && !isValid; lower++)
            {
                for (int upper = lower; upper < i && !isValid; upper++)
                {
                    if (lower == upper)
                    {
                        continue;
                    }

                    if (data[lower] + data[upper] == number)
                    {
                        isValid = true;
                    }
                }
            }

            if (!isValid)
            {
                return number;
            }
        }

        throw new();
    }

    var part1 = findInvalidNumber();

    Console.WriteLine($"Part 1: {part1}");

    long findEncryptionWeakness()
    {
        for (int i = 0; i < data.Count; i++)
        {
            var val = 0L;

            for (int j = i + 1; j < data.Count; j++)
            {
                val += data[j];

                if (val == part1)
                {
                    var range = data.Skip(i).Take(j - i + 1).ToArray();
                    return range.Min() + range.Max();
                }

                if (val > part1)
                {
                    break;
                }
            }
        }

        throw new();
    }

    Console.WriteLine($"Part 2: {findEncryptionWeakness()}\n");
}
