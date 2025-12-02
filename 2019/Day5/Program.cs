// Advent of Code challenge: https://adventofcode.com/2019/day/5
Console.WriteLine("AoC - Day 5\n\n");

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    Console.WriteLine($"[{inputFile}]\n");

    var program = File.ReadAllText(inputFile)
        .Split(',', StringSplitOptions.RemoveEmptyEntries)
        .Select(int.Parse)
        .ToList();

    Console.WriteLine($"Part 1:");
    Console.WriteLine($"Part 2:\n");
}

return;

static int RunProgram(List<int> program, int noun, int verb)
{
    var localProgram = program.ToArray();

    localProgram[1] = noun;
    localProgram[2] = verb;
    
    var position = 0;

    try
    {
        while (localProgram[position] != 99)
        {
            var position1 = localProgram[position + 1];
            var position2 = localProgram[position + 2];
            var targetPosition = localProgram[position + 3];

            localProgram[targetPosition] = localProgram[position] switch
            {
                1 => localProgram[position1] + localProgram[position2],
                2 => localProgram[position1] * localProgram[position2],
                _ => throw new Exception($"Invalid opcode [{position}]: {localProgram[position]}")
            };

            position += 4;
        }
    }
    catch (IndexOutOfRangeException)
    {
        return -1;
    }
    
    return localProgram[0];
}
