using AoC.Shared.Collections;
using AoC.Shared.Enumerable;
using AoC.Shared.Strings;

Console.WriteLine("AOC - Day 16\n\n");

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    Console.WriteLine($"[{inputFile}]\n");

    var bits = File.ReadAllText(inputFile)
        .Trim()
        .Chunk(2)
        .SelectMany(chars => {
            var @byte = Convert.FromHexString(string.Concat(chars)).First();
            var bits = new bool[8];
            
            foreach(var i in Enumerable.Range(0,8))
            {
                bits[i] = @byte.GetBit(i);
            }

            return bits.Reverse();
        })
        .ToArray();

    var versionSum = 0;

    long parseBITS(bool[] bits, out int processed)
    {
        var version = bits[..3].Reverse().ToInt32();
        var typeId = bits[3..6].Reverse().ToInt32();
        
        versionSum += version;        
        processed = 6;

        if (typeId == 4)
        {
            var numberBits = new List<bool>();

            while(true)
            {
                numberBits.AddRange(bits[(processed + 1)..(processed + 5)]);
                processed += 5;

                if (bits[processed - 5] == false)
                {
                    break;
                }
            }

            return numberBits.ToArray().Reverse().ToInt64();
        }
        else
        {
            var byCount = bits[processed];
            var lengthBits = byCount ? 11 : 15;
            processed++;
            
            var length = bits[processed..(processed + lengthBits)].Reverse().ToInt32();
            processed += lengthBits;

            var numbers = new List<long>();

            do
            {
                numbers.Add(parseBITS(bits[processed..], out var packetBits));
                processed += packetBits;

                length -= byCount ? 1 : packetBits;
            }
            while (length > 0);

            return typeId switch
            {
                0 => numbers.Sum(),
                1 => numbers.Product(),
                2 => numbers.Min(),
                3 => numbers.Max(),
                5 => numbers.First() > numbers.Last() ? 1 : 0,
                6 => numbers.First() < numbers.Last() ? 1 : 0,
                7 => numbers.First() == numbers.Last() ? 1 : 0,
                _ => throw new Exception("Unknown type")
            };
        }
    }

    var part2 = parseBITS(bits, out _);

    Console.WriteLine($"Part 1: {versionSum}");
    Console.WriteLine($"Part 2: {part2}\n");
}