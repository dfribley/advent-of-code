using AoC.Shared.Collections;
using AoC.Shared.Enumerable;
using AoC.Shared.Ranges;
using AoC.Shared.Strings;

Console.WriteLine("AOC - Day 5\n\n");

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    Console.WriteLine($"[{inputFile}]\n");

    var input = File.ReadAllLines(inputFile)
        .ToList();

    var seeds = input.First().Split(":")[1]
        .Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(s => s.ToInt64());

    var maps = input.Skip(2)
        .Split(string.IsNullOrEmpty)
        .Select(g =>
        {
            var map = g.Values.First().Split(" ")[0].Split("-");
            var translations = g.Values.Skip(1).Select(r =>
            {
                var vals = r.Split(" ", StringSplitOptions.RemoveEmptyEntries);

                return (
                    srcRange: new Range64(vals[1].ToInt64(), vals[2].ToInt64()),
                    destStart: vals[0].ToInt64());

            }).ToArray();

            return (source: map[0], destination: map[2], translations);
        })
        .ToDictionary(t => t.source, t => t);

    long transformSingle(long value)
    {
        var type = "seed";

        do
        {
            var (source, destination, translations) = maps[type];

            foreach (var (srcRange, destStart) in translations)
            {
                if (srcRange.Contains(value))
                {
                    value = destStart + (value - srcRange.Start);
                    break;
                }
            }

            type = destination;

        } while (type != "location");

        return value;
    }

    long transformRange(Range64 startRange)
    {
        var type = "seed";
        var translated = new List<Range64>();
        var queue = new Queue<Range64>();
        queue.Enqueue(startRange);

        while (true)
        {
            var (source, destination, translations) = maps[type];

            while(queue.Any())
            {
                var range = queue.Dequeue();

                var unmapped = true;
                foreach (var (srcRange, destStart) in translations)
                {
                    if (range.Overlaps(srcRange))
                    {
                        if (range.Start < srcRange.Start)
                        {
                            queue.Enqueue(new Range64(range.Start, srcRange.Start - range.Start));
                        }

                        if (range.End > srcRange.End)
                        {
                            queue.Enqueue(new Range64(srcRange.End + 1, range.End - srcRange.End));
                        }

                        var left = range.Start < srcRange.Start ? srcRange.Start : range.Start;
                        var right = range.End > srcRange.End ? srcRange.End : range.End;

                        translated.Add(new Range64(destStart + left - srcRange.Start, right - left + 1));

                        unmapped = false;
                        break;
                    }
                }

                if (unmapped)
                {
                    translated.Add(range);
                }
            }

            if (destination == "location")
            {
                break;
            }

            queue.Add(translated);
            translated.Clear();
            type = destination;
        }

        return translated.Select(r => r.Start).Min();
    }

    Console.WriteLine($"Part 1: {seeds.Select(transformSingle).Min()}");

    var pairedSeeds = seeds
        .Split(2)
        .Select(grp => new Range64(grp.Values.First(), grp.Values.Last()))
        .ToList();

    Console.WriteLine($"Part 2: {pairedSeeds.Select(transformRange).Min()}\n");
}
