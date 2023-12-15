Console.WriteLine("AOC - Day 7\n\n");

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    Console.WriteLine($"[{inputFile}]\n");

    var input = File.ReadAllLines(inputFile)
        .Where(line => !string.IsNullOrEmpty(line))
        .ToList();

    Console.WriteLine($"Part 1:");
    Console.WriteLine($"Part 2:\n");
}
