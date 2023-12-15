Console.WriteLine("AOC - Puzzle 6\n\n");

static int indexOfUniqueMarker(string stream, int markerSize)
{
    var markerQueue = new Queue<char>();

    for (var i = 0; i < stream.Length; i++)
    {
        markerQueue.Enqueue(stream[i]);

        if (markerQueue.Count > markerSize)
        {
            markerQueue.Dequeue();
        }

        if (markerQueue.Distinct().Count() == markerSize)
        {
            return i;
        }
    }

    throw new Exception("Marker not found!");
};

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    Console.WriteLine($"[{inputFile}]\n");
    
    var stream = File.ReadAllText(inputFile);
    
    var part1 = indexOfUniqueMarker(stream, 4) + 1;
    Console.WriteLine($"Part 1: {part1}");

    var part2 = indexOfUniqueMarker(stream, 14) + 1;
    Console.WriteLine($"Part 2: {part2}\n");
}