Console.WriteLine("AOC - Day 15\n\n");

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    Console.WriteLine($"[{inputFile}]\n");

    var numbers = File.ReadAllText(inputFile)
        .Split(',')
        .Select(int.Parse)
        .ToArray();

    var lastSpoken = new Dictionary<int, int>();
    for (var i = 0; i < numbers.Length - 1; i++)
    {
        lastSpoken[numbers[i]] = i + 1;
    }

    var turn = numbers.Length;
    var lastNumber = numbers[^1];

    while (turn < 30000000)
    {
        var myNumber = 0;

        if (lastSpoken.TryGetValue(lastNumber, out var value))
        {
            myNumber = turn - value;
        }

        lastSpoken[lastNumber] = turn;
        lastNumber = myNumber;
        turn++;

        if (turn == 2020)
        {
            Console.WriteLine($"Part 1: {myNumber}");
        }
    }

    Console.WriteLine($"Part 2: {lastNumber}\n");
}
