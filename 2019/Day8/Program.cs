// Advent of Code challenge: https://adventofcode.com/2019/day/8

using AoC.Shared.Enumerable;
using AoC.Shared.PixelWriter;
using AoC.Shared.Strings;

Console.WriteLine("AoC - Day 8\n\n");

foreach (var inputFile in new[] { "input.txt" })
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    Console.WriteLine($"[{inputFile}]\n");
    
    const int wide = 25;
    const int tall = 6;
    
    var layers = File.ReadAllText(inputFile)
        .Trim()
        .Split(wide * tall)
        .ToList();
    
    var part1 = layers
        .Select(grp =>
        {
            var zeros = 0;
            var ones = 0;
            var twos = 0;

            foreach (var v in grp.Values)
            {
                switch (v.ToInt32())
                {
                    case 0:
                        zeros++;
                        break;
                    case 1:
                        ones++;
                        break;
                    case 2:
                        twos++;
                        break;
                }
            }

            return (zeros, ones, twos);
        })
       .OrderBy(x => x.zeros)
       .Select(t => t.ones * t.twos)
       .First();
    
    Console.WriteLine($"Part 1: {part1}");

    var image = new int[tall, wide];

    for (var t = 0; t < tall; t++)
    {
        for (var w = 0; w < wide; w++)
        {
            foreach (var pixel in layers.Select(layer => layer.Values.ElementAt(w + t * wide).ToInt32()))
            {
                switch (pixel)
                {
                    case 0:
                    case 1:
                        image[t, w] = pixel;
                        break;
                    case 2:
                        continue;
                }

                break;
            }
        }
    }

    Console.WriteLine("Part 2:\n");
    
    var pw = new PixelWriter(wide);
    for (var t = 0; t < tall; t++)
    {
        for (var w = 0; w < wide; w++)
        {
            pw.Write(image[t, w] == 1 ? '#' : ' ');

            if (w < wide - 1 && (w + 1) % 5 == 0)
            {
                Console.Write("  ");
            }
        }
    }
}
