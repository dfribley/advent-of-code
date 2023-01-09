using AoC.Day13;
using Shared.Enumerable;

Console.WriteLine("AOC - Day 13\n");

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    Console.WriteLine($"[{inputFile}]\n");
    
    var comparer = new PacketComparer();
    var packets = File.ReadAllLines(inputFile)
        .Where(line => !string.IsNullOrEmpty(line))
        .Select(line => line.ToPacket())
        .ToList();

    var part1 = packets.Split(2)
        .Where(sg => comparer.Compare(sg.Values.First(), sg.Values.Last()) == -1)
        .Select(sg => sg.Id + 1)
        .Sum();
    Console.WriteLine($"Part 1: {part1}");

    var divPacket1 = "[[2]]".ToPacket();
    var divPacket2 = "[[6]]".ToPacket();
    var part2 = 1;

    packets.Add(divPacket1);
    packets.Add(divPacket2);

    packets.Order(comparer)
        .Select((packet, i) => (packet, i))
        .Where(t => t.packet == divPacket1 || t.packet == divPacket2)
        .ToList()
        .ForEach(t => part2 *= t.i + 1);
    Console.WriteLine($"Part 2: {part2}\n");
}
