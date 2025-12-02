// Advent of Code challenge: https://adventofcode.com/2019/day/2
Console.WriteLine("AoC - Day 2\n\n");

foreach (var inputFile in new[] { /*"sample.txt",*/ "input.txt" })
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

    Console.WriteLine($"Part 1: {RunProgram(program, 12, 2)}");

    var found = false;

    for (var n = 0; n < 100; n++)
    {
        for (var v = 0; v < 100; v++)
        {
            if (RunProgram(program, n, v) == 19690720)
            {
                Console.WriteLine($"Part 2: {100*n+v}\n");
                found = true;
            }
        }

        if (found)
        {
            break;
        }
    }
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
