using System.Text;
using AoC.Shared.BinaryString;
using AoC.Shared.Strings;

Console.WriteLine("AOC - Day 14\n\n");

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    Console.WriteLine($"[{inputFile}]\n");

    var instructions = File.ReadAllLines(inputFile)
        .Where(line => !string.IsNullOrEmpty(line) && !line.StartsWith('#'))
        .ToArray();

    long Emulate(bool part1 = true)
    {
        var mask = string.Empty;
        var memory = new Dictionary<long, long>();
    
        foreach (var instruction in instructions)
        {
            var parts = instruction.Split(" = ");

            if (parts[0] == "mask")
            {
                mask = parts[1];
            }
            else
            {
                var memAddress = parts[0].Split("[")[1][..^1].ToInt32();
                var value = parts[1].ToInt32().ToBinaryString(36);
            
                if (part1)
                {
                    memory[memAddress] = value.ApplyMask(mask, 'X').ToLong();
                    continue;
                }

                var decoder = memAddress.ToBinaryString(36).ApplyMask(mask, '0');
                
                WriteToMemoryWithDecoder(decoder.ToString(), value.ToInt64());
            }
        }
        
        return memory.Values.Sum();
        
        void WriteToMemoryWithDecoder(string decoder, long value)
        {
            var floatIndex = decoder.IndexOf('X');
        
            if (floatIndex == -1)
            {
                memory[Convert.ToInt64(decoder, 2)] = value;
            }
            else
            {
                var sb = new StringBuilder(decoder);
                
                sb[floatIndex] = '0';
                WriteToMemoryWithDecoder(sb.ToString(), value);
                
                sb[floatIndex] = '1';
                WriteToMemoryWithDecoder(sb.ToString(), value);
            }
        }
    }

    //Console.WriteLine($"Part 1: {Emulate()}");
    Console.WriteLine($"Part 2: {Emulate(false)}\n");
}