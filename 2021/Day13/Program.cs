using System.Numerics;
using AoC.Shared.Enumerable;
using AoC.Shared.PixelWriter;
using AoC.Shared.Strings;

Console.WriteLine("AOC - Day 13\n\n");

static void foldThePaper(HashSet<Vector2> dots, (string axis, int value) fold)
{
    dots.ToList()
        .ForEach(d =>
        {
            var shift = default(Vector2);

            if (fold.axis == "x" && d.X > fold.value)
            {
                shift.X = (d.X - fold.value) * 2;
            }
            else if (fold.axis == "y" && d.Y > fold.value)
            {
                shift.Y = (d.Y - fold.value) * 2;
            }

            if (shift != default)
            {
                dots.Remove(d);
                dots.Add(d - shift);
            }    
        });
}

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    Console.WriteLine($"[{inputFile}]\n");

    var input = File.ReadAllLines(inputFile)
        .Split(string.IsNullOrEmpty)
        .ToList();

    var dots = input[0].Values
        .Select(line =>
        {
            var parts = line.Split(',');

            return new Vector2(parts[0].ToInt32(), parts[1].ToInt32());
        })
        .ToHashSet();

    var folds = input[1].Values
        .Select(line =>
        {
            var parts = line.Split(' ')[2].Split('=');

            return (parts[0], parts[1].ToInt32());
        })
        .ToList();

    foldThePaper(dots, folds.First());
    Console.WriteLine($"Part 1: {dots.Count}");

    foreach (var fold in folds.Skip(1))
    {
        foldThePaper(dots, fold);
    }

    Console.WriteLine("Part 2:");
    PixelWriterHelper.WriteCollection(dots);
    Console.WriteLine();
}