using AoC.Shared.Strings;
using System.Diagnostics;

Console.WriteLine("AOC - Day 20\n");

static long mixFile(IList<long> file, int rounds)
{
    var indexesProcessed = Enumerable.Range(0, file.Count).ToList();

    while (rounds-- > 0)
    {
        for (var ix = 0; ix < indexesProcessed.Count; ix++)
        {
            var value = file[indexesProcessed[ix]];
            var newIndex = indexesProcessed[ix] + value;

            file.RemoveAt(indexesProcessed[ix]);

            // Todo: Add index wrap helper to shared with mod support
            if (newIndex <= 0)
            {
                newIndex = file.Count - (Math.Abs(newIndex) % file.Count);
            }
            else if (newIndex >= file.Count)
            {
                newIndex = 0 + (newIndex % file.Count);
            }

            indexesProcessed
                .Select((v, i) => Tuple.Create(i, v))
                .Where(t => t.Item2 >= indexesProcessed[ix])
                .ToList()
                .ForEach(t =>
                {
                    indexesProcessed[t.Item1]--;
                });

            file.Insert((int)newIndex, value);
            indexesProcessed
                .Select((v, i) => Tuple.Create(i, v))
                .Where(t => t.Item2 >= newIndex)
                .ToList()
                .ForEach(t =>
                {
                    indexesProcessed[t.Item1]++;
                });

            indexesProcessed[ix] = (int)newIndex;
        }
    }

    var zeroIndex = file.IndexOf(0);

    return new[] { 1000, 2000, 3000 }
        .Select(t => file[(zeroIndex + t) % file.Count])
        .Sum();
}

foreach (var input in new[] { "sample.txt", "input.txt" })
{
    Console.WriteLine($"[{input}]\n");

    var file = File.ReadLines(input)
        .Select(l => l.ToInt64())
        .ToList();

    var sw = new Stopwatch();
    sw.Start();
    var part1 = mixFile(new List<long>(file), 1);
    sw.Stop();

    Console.WriteLine($"Part 1: ({string.Format("{0:00}:{1:00}.{2:00}", sw.Elapsed.Minutes, sw.Elapsed.Seconds, sw.Elapsed.Milliseconds / 10)}) {part1}");

    var decryptionKey = 811589153;
    file = file.Select(n => n * decryptionKey).ToList();

    sw.Restart();
    var part2 = mixFile(file, 10);
    sw.Stop();

    Console.WriteLine($"Part 2: ({string.Format("{0:00}:{1:00}.{2:00}", sw.Elapsed.Minutes, sw.Elapsed.Seconds, sw.Elapsed.Milliseconds / 10)}) {part2}\n");
}