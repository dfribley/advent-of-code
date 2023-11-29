using AoC.Shared.BinarySearch;

Console.WriteLine("AOC - Day 5\n\n");

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    Console.WriteLine($"[{inputFile}]\n");

    int toSeatId(string boardingPass)
    {
        var set = new BinarySearchSet(0, 127);
        boardingPass[..6]
            .ToList()
            .ForEach(c => set = c == 'F' ? set.LowerHalf : set.UpperHalf);
        var row = boardingPass[6] == 'F' ? set.Low : set.High;

        set = new BinarySearchSet(0, 7);
        boardingPass[7..10]
            .ToList()
            .ForEach(c => set = c == 'L' ? set.LowerHalf : set.UpperHalf);
        var column = boardingPass[9] == 'L' ? set.Low : set.High;

        return row * 8 + column;
    }

    var seatIds = File.ReadAllLines(inputFile)
        .Where(line => !string.IsNullOrEmpty(line))
        .Select(toSeatId)
        .Order()
        .ToArray();

    Console.WriteLine($"Part 1: { seatIds.Last() }");

    var mySeat = "Unknown";
    for (int i = 0; i < seatIds.Length - 2; i++)
    {
        if(seatIds[i] + 2 == seatIds[i + 1])
        {
            mySeat = $"{ seatIds[i] + 1 }";
            break;
        }
    }

    Console.WriteLine($"Part 2: { mySeat }\n");
}
