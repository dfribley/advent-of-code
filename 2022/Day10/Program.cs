using AoC.Shared.PixelWriter;
using AoC.Shared.Strings;

Console.WriteLine("AOC - Day 10\n");

var pixelWriter = new PixelWriter(40);

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    Console.WriteLine($"[{inputFile}]\n");

    var cycles = new Dictionary<int, int>();
    var cycle = 0;
    var register = 1;

    File.ReadAllLines(inputFile)
        .ToList()
        .ForEach(l =>
        {
            var parts = l.Split(' ');

            if (parts[0] == "noop")
            {
                cycle++;
                cycles.Add(cycle, register);
            }
            else
            {
                cycles.Add(++cycle, register);
                cycles.Add(++cycle, register);

                register += parts[1].ToInt32();
            }
        });

    var part1 = new[] { 20, 60, 100, 140, 180, 220 }
        .Select(i => i * cycles[i])
        .Sum();
    Console.WriteLine($"Part 1: {part1}");

    Console.WriteLine("Part 2:");
    foreach (var kvp in cycles)
    {
        if (Math.Abs(kvp.Value - ((kvp.Key - 1) % 40)) <= 1)
        {
            pixelWriter.Write('#');
        }
        else
        {
            pixelWriter.Write(' ');
        }
    }
    Console.WriteLine();
}